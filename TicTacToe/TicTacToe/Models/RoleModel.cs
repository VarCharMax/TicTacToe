using Microsoft.AspNetCore.Identity;
using System;

namespace TicTacToe.Models
{
    public class RoleModel : IdentityRole<Guid>
    {
        public RoleModel()
        {
        }

        public RoleModel(string roleName) : base(roleName)
        {
        }
    }
}
