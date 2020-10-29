using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Extensions;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class GameSessionService : IGameSessionService
    {
        private static ConcurrentBag<GameSessionModel> _sessions;
        private IUserService _userService;

        public GameSessionService(IUserService userService)
        {
            _userService = userService;
        }

        static GameSessionService()
        {
            _sessions = new ConcurrentBag<GameSessionModel>();
        }

        public Task<GameSessionModel> CreateGameSession(Guid invitationId, UserModel invitedBy, UserModel invitedPlayer)
        {
            GameSessionModel session = new GameSessionModel
            {
                User1 = invitedBy,
                User2 = invitedPlayer,
                Id = invitationId,
                ActiveUser = invitedBy
            };

            _sessions.Add(session);

            return Task.FromResult(session);
        }

        public async Task<GameSessionModel> GetGameSession(Guid gameSessionId)
        {
            return await Task.Run(() => _sessions.FirstOrDefault(x => x.Id == gameSessionId));
        }

        public Task<GameSessionModel> AddTurn(Guid id, UserModel user, int x, int y)
        {
            var gameSession = _sessions.FirstOrDefault(session => session.Id == id);

            List<TurnModel> turns;

            if (!gameSession.Turns.IsNullOrEmpty())
            {
                turns = new List<TurnModel>(gameSession.Turns);
            }
            else
            {
                turns = new List<TurnModel>();
            }

            turns.Add(new TurnModel { User = user, X = x, Y = y, IconNumber = user.Email == gameSession.User1?.Email ? "1" : "2" });

            gameSession.Turns = turns;
            gameSession.TurnNumber += 1;

            if (gameSession.User1?.Email == user.Email)
            {
                gameSession.ActiveUser = gameSession.User2;
            }
            else
            {
                gameSession.ActiveUser = gameSession.User1;
            }

            gameSession.TurnFinished = true;

            _sessions = new ConcurrentBag<GameSessionModel>(_sessions.Where(u => u.Id != id)) { gameSession };
            
            return Task.FromResult(gameSession);
        }
    }
}
