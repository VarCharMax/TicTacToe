using System;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public interface IGameSessionService
    {
        Task<GameSessionModel> GetGameSession(Guid gameSessionId);
        Task<GameSessionModel> CreateGameSession(Guid invitationId, UserModel invitedBy, UserModel invitedPlayer);
        Task<GameSessionModel> AddTurn(Guid id, UserModel user, int x, int y);
    }
}