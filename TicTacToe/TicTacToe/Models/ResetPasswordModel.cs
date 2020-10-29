namespace TicTacToe.Models
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
