using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Produces("application/json")]
    [Route("restapi/v1/GameInvitation")]
    [ApiController]
    public class GameInvitationApiController : ControllerBase
    {
        private IGameInvitationService _gameInvitationService;
        private IUserService _userService;

        public GameInvitationApiController(IGameInvitationService gameInvitationService, IUserService userService)
        {
            _gameInvitationService = gameInvitationService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<GameInvitationModel>> Get()
        {
            return await _gameInvitationService.All();
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<GameInvitationModel> Get(Guid id)
        {
            return await _gameInvitationService.Get(id);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GameInvitationModel invitation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var invitedPlayer = await _userService.GetUserByEmail(invitation.EmailTo);

            if (invitedPlayer == null) return BadRequest();

            await _gameInvitationService.Add(invitation);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]GameInvitationModel invitation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var invitedPlayer = await _userService.GetUserByEmail(invitation.EmailTo);

            if (invitedPlayer == null) return BadRequest();

            await _gameInvitationService.Update(invitation);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async void Delete(Guid id)
        {
            await _gameInvitationService.Delete(id);
        }
    }
}
