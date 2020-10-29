using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Models
{
    public class GameSessionModel
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(UserId1))]
        public Guid UserId1 { get; set; }
        public Guid UserId2 { get; set; }
        public UserModel User1 { get; set; }
        public UserModel User2 { get; set; }
        public IEnumerable<TurnModel> Turns { get; set; }
        [NotMapped]
        public UserModel Winner { get; set; }
        public Guid WinnerId { get; set; }
        [NotMapped]
        public UserModel ActiveUser { get; set; }
        public Guid ActiveUserID { get; set; }
        public bool TurnFinished { get; set; }
        public int TurnNumber { get; set; }
    }
}
