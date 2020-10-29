using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace TicTacToe.TagHelpers
{
    [HtmlTargetElement("Gravatar")]
    public class GravatarTagHelper : TagHelper
    {
        private ILogger<GravatarTagHelper> _logger;

        public GravatarTagHelper(ILogger<GravatarTagHelper> logger)
        {
            _logger = logger;
        }

        public string Email { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            byte[] photo = null;

            if (CheckIsConnected())
            {
                photo = GetPhoto(Email);
            }
            else
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "no-photo.jpg");

                if (File.Exists(filePath))
                {
                    photo = File.ReadAllBytes(filePath);
                }
            }

            if (photo != null && photo.Length > 0)
            {
                output.TagName = "img";
                output.Attributes.SetAttribute("src", $"data:image/jpeg;base64,{Convert.ToBase64String(photo)}");
            }
        }

        private bool CheckIsConnected()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var gravatarResponse = httpClient.GetAsync("https://gravatar.com/avatar").Result;

                    return gravatarResponse.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot check the gravatar service status: {ex}");

                return false;
            }
        }

        private byte[] GetPhoto(string email)
        {
            var httpClient = new HttpClient();

            return httpClient.GetByteArrayAsync(new Uri($"https://www.gravatar.com/avatar/{HashEmailForGravatarString(email)}")).Result;
        }

        private static string HashEmailForGravatarString(string email)
        {
            var md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(email.ToLower()));

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2")); //Hexadecimal
            }

            return stringBuilder.ToString();
        }
    }
}
