using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TicTacToe.Data;
using TicTacToe.Extensions;
using TicTacToe.Filters;
using TicTacToe.Managers;
using TicTacToe.Models;
using TicTacToe.Monitoring;
using TicTacToe.Options;
using TicTacToe.Services;
using TicTacToe.ViewEngines;

namespace TicTacToe
{
    public class Startup
    {
        public IConfiguration _configuration { get; set; }
        public IWebHostEnvironment _hostingEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public void ConfigureCommonServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(_configuration);

            var section = _configuration.GetSection("Monitoring");

            var monitoringOptions = new MonitoringOptions();
            section.Bind(monitoringOptions);

            if (monitoringOptions.MonitoringType == "azureapplicationinsights")
            {
                services.AddSingleton<IMonitoringService, AzureApplicationInsightsMonitoringService>();
            }
            else if (monitoringOptions.MonitoringType == "amazonwebservicescloudwatch")
            {
                services.AddSingleton<IMonitoringService, AmazonWebServicesMonitorService>();
            }

            services.AddLocalization(options => options.ResourcesPath = "Localization");

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
            });

            services.AddControllersWithViews(o => {
                o.Filters.Add(typeof(DetectMobileFilter));
            });

            services.AddOptions();
            services.Configure<EmailServiceOptions>(_configuration.GetSection("Email"));
            services.AddEmailService(_hostingEnvironment, _configuration);
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IGameInvitationService, GameInvitationService>();
            services.AddTransient<ApplicationUserManager>();

            services.AddHttpContextAccessor();

            services.AddTransient<IEmailTemplateRenderService, EmailTemplateRenderService>();
            services.AddTransient<IEmailViewEngine, EmailViewEngine>();
            
            services.AddRouting();
            services.AddSession(o => {
                o.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddScoped<IGameSessionService, GameSessionService>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<GameDbContext>((serviceProvider, options) => {
                    options.UseSqlServer(connectionString).UseInternalServiceProvider(serviceProvider);
                });

            services.AddScoped(typeof(DbContextOptions<GameDbContext>), (serviceProvider) => {
                return new DbContextOptionsBuilder<GameDbContext>().UseSqlServer(connectionString).Options;
            });
            
            services.AddIdentity<UserModel, RoleModel>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<GameDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = _hostingEnvironment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            })
            .AddFacebook(facebook => {

                var fbAuth = _configuration.GetSection("Authentication:Facebook");

                facebook.AppId = fbAuth["AppId"];
                facebook.AppSecret = fbAuth["AppSecret"];
                facebook.SignInScheme = IdentityConstants.ExternalScheme;
                facebook.AccessDeniedPath = "/AccessDenied";
                // facebook.ClientId = "619342225353363";
                // facebook.ClientSecret = "16bef6eb2046dfa94ef2858cf584f6e8";
            })
            .AddGoogle(google => {
                var googleAuth = _configuration.GetSection("Authentication:Google");

                google.ClientId = googleAuth["ClientId"];
                google.ClientSecret = googleAuth["ClientSecret"];
                google.SignInScheme = IdentityConstants.ExternalScheme;
            });

            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                options => options.ResourcesPath = "Localization")
                .AddDataAnnotationsLocalization();

            services.AddAuthorization(options => {
                options.AddPolicy("AdministratorAccessLevelPolicy", policy => policy.RequireClaim("AccessLevel", "Administrator"));
            });
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Learning ASP.NET Core 3.0 Rest-API", Version = "v1", Description = "Demonstrating auto-generated API documentation", Contact = new OpenApiContact { Name = "Rohan Parkes", Email = "example@example.com" }, License = new OpenApiLicense { Name = "MIT" } });
            });
        }

        public void ConfigureStagingServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureCommonServices(services);
        }

        /*
        private static void ApiPipeline(IApplicationBuilder app)
        {
            app.Run(async context => {
                await context.Response.WriteAsync("Branched to Api Pipeline");
            });
        }

        private static void WebPipeline(IApplicationBuilder app)
        {
            app.MapWhen(context => {
                return context.Request.Query.ContainsKey("usr");
            }, UserPipeline);

            app.Run(async context => {
                await context.Response.WriteAsync("Branched to Web Pipeline");
            });
        }

        private static void UserPipeline(IApplicationBuilder app)
        {
            app.Run(async context => {
                await context.Response.WriteAsync("Branched to User Pipeline");
            });
        }
        */

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DiagnosticListener diagnosticListener)
        {
            /*
            app.Map("/api", ApiPipeline);
            app.Map("/web", WebPipeline);
            app.Use(next => async context => {
                await context.Response.WriteAsync("Called Use.");
                await next.Invoke(context);
            });

            app.Run(async context => {
                await context.Response.WriteAsync("Finished with Run");
            });
            */

            var listener = new ApplicationDiagnosticListener();
            diagnosticListener.SubscribeWithAdapter(listener);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                // app.UseExceptionHandler("/Error");
                // app.UseHsts();
            }

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("CreateUser", context => {
                var firstName = context.Request.Query["firstName"];
                var lastName = context.Request.Query["lastName"];
                var email = context.Request.Query["email"];
                var password = context.Request.Query["password"];

                var userService = context.RequestServices.GetService<IUserService>();

                userService.RegisterUser(new UserModel { FirstName = firstName, LastName = lastName, Password = password, Email = email, EmailConfirmed = true });

                return context.Response.WriteAsync($"User {firstName} {lastName} has been successfully created.");
            });

            var newUserRoutes = routeBuilder.Build();
            app.UseRouter(newUserRoutes);

            using (StreamReader iisUrlRewriteStreamReader = File.OpenText("Rewrite/IISUrlRewrite.xml"))
            {
                var options = new RewriteOptions().AddIISUrlRewrite(iisUrlRewriteStreamReader); //.AddRewrite("NewUser", "/UserRegistration/Index", false);
                app.UseRewriter(options);
            }

            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var localizationOptions = new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new CultureProviderResolverService());

            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseCommunicationMiddleware();
            // app.UseDirectoryBrowser();
            app.UseRequestLocalization(localizationOptions);
            app.UseStatusCodePages("text/plain", "HTTP Error Status Code: {0}");
            /*
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LEARNING ASP.CORE 3.0 V1");
            });
            */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(name: "areas", areaName: "Account", pattern: "Account/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            /*
            var provider = app.ApplicationServices;
            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            scope.ServiceProvider.GetRequiredService<GameDbContext>()
                .Database.Migrate();
            */
        }
    }
}
