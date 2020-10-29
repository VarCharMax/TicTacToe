using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Models
{
    public class GameInvitationModel
    {
        [Key]
        public Guid Id { get; set; }
        public string EmailTo { get; set; }
        public string InvitedBy { get; set; }
        public UserModel InvitedByUser { get; set; }
        [ForeignKey(nameof(InvitedById))]
        public Guid InvitedById { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
}
