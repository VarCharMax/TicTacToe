using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class UserRoleModel : IdentityUserRole<Guid>
    {
        [Key]
        public Guid Id { get; set; }
    }
}
