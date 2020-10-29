using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class GameInvitationService : IGameInvitationService
    {
        private static ConcurrentBag<GameInvitationModel> _gameInvitations;

        static GameInvitationService()
        {
            _gameInvitations = new ConcurrentBag<GameInvitationModel>();
        }

        public Task<GameInvitationModel> Add(GameInvitationModel gameInvitationModel)
        {
            // gameInvitationModel.Id = Guid.NewGuid();
            _gameInvitations.Add(gameInvitationModel);

            return Task.FromResult(gameInvitationModel);
        }

        public Task Update(GameInvitationModel gameInvitationModel)
        {
            _gameInvitations = new ConcurrentBag<GameInvitationModel>(_gameInvitations.Where(x => x.Id != gameInvitationModel.Id)) { gameInvitationModel };

            return Task.CompletedTask;
        }

        public Task<GameInvitationModel> Get(Guid Id)
        {
            return Task.FromResult(_gameInvitations.FirstOrDefault(x => x.Id == Id));
        }

        public Task<IEnumerable<GameInvitationModel>> All()
        {
            return Task.FromResult<IEnumerable<GameInvitationModel>>(_gameInvitations.ToList());
        }

        public Task Delete(Guid id)
        {
            _gameInvitations = new ConcurrentBag<GameInvitationModel>(_gameInvitations.Where(x => x.Id != id ));

            return Task.CompletedTask;
        }
    }
}
