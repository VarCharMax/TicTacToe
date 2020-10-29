using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class UserModel : IdentityUser<Guid>
    {
        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "FirstNameRequired")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        [Required(ErrorMessage = "LastNameRequired")]
        public string LastName { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "PasswordRequired"), DataType(DataType.Password)]
        public string Password { get; set; }
        public DateTime? EmailConfirmationDate { get; set; }
        public int Score { get; set; }
    }
}
