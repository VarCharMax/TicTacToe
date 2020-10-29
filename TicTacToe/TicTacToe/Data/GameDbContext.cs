using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.Extensions;
using TicTacToe.Models;

namespace TicTacToe.Data
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameInvitationModel> GameInvitationModels { get; set; }
        public DbSet<GameSessionModel> GameSessionModels { get; set; }
        public DbSet<TurnModel> TurnModels { get; set; }
        public DbSet<UserModel> UserModels { get; set; }
        public DbSet<RoleModel> RoleModel { get; set; }
        public DbSet<TwoFactorCodeModel> TwoFactorCodeModels { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.Entity(typeof(GameSessionModel))
                .HasOne(typeof(UserModel), "User2")
                .WithMany()
                .HasForeignKey("UserId2")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity(typeof(GameSessionModel))
                .HasOne(typeof(UserModel), "Winner")
                .WithMany()
                .HasForeignKey("WinnerId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("ExternalLogin").HasKey(p => new { p.UserId });
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoleModel").HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
