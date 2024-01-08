using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Places.Dto;
using Places.Interfaces;
using Places.Models;
using Places.Repository;

namespace Places.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IMapper _mapper;
        public ConnectionController(IConnectionRepository connectionRepository,IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _connectionRepository = connectionRepository;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Connection>))]
        public IActionResult GetConnections()
        {
            var connections = _mapper.Map<List<ConnectionDto>>(_connectionRepository.GetConnections());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(connections);
        }

        [HttpGet("{connectionId}")]
        [ProducesResponseType(200, Type = typeof(Connection))]
        [ProducesResponseType(400)]
        public IActionResult GetConnection(int connectionId)
        {
            if (!_connectionRepository.ConnectionExists(connectionId))
                return NotFound();

            var connection = _mapper.Map<ConnectionDto>(_connectionRepository.GetConnection(connectionId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(connection);
        }

        [HttpGet("CheckConnectionBetweenUsers")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult CheckConnectionBetweenUsers(int senderId, int receiverId)
        {
            if (_connectionRepository.CheckConnectionBetweenUsers(senderId, receiverId))
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
                
        }

        //[HttpGet("GetConnectionOfAUser/{userProfileId}")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Connection>))]
        //[ProducesResponseType(400)]
        //public IActionResult GetConnectionOfAUser(int userProfileId)
        //{
        //    var user = _userProfileRepository.GetUserProfile(userProfileId);
        //    if(user == null)
        //    {
        //        return NotFound();
        //    }
        //    var connections = _connectionRepository.GetConnectionOfAUser(userProfileId).ToList();
            
        //    var connectionsDTO = _mapper.Map<List<ConnectionDto>>(connections);
        //    if(connectionsDTO == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(connectionsDTO);
        //}

        [HttpGet("GetConnectionOfAUser/{userProfileId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserProfile>))]
        [ProducesResponseType(400)]
        public IActionResult GetConnectionOfAUser(int userProfileId)
        {
            var user = _userProfileRepository.GetUserProfile(userProfileId);
            if (user == null)
            {
                return NotFound();
            }
            var connections = _connectionRepository.GetConnectionOfAUser(userProfileId).ToList();

            List<UserProfile> connectedUsers = new List<UserProfile>();
            foreach (var connection in connections)
            {
                var userProfile = _userProfileRepository.GetUserProfile((int)connection.ReceiverId);
                connectedUsers.Add(userProfile);
            }

            var userProfilesDto = _mapper.Map<List<UserProfileDto>>(connectedUsers);
            if (userProfilesDto == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfilesDto);
        }

        [HttpGet("GetPendingConnections/{userProfileId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Connection>))]
        [ProducesResponseType(400)]
        public IActionResult GetPendingConnections(int userProfileId)
        {
            var user = _userProfileRepository.GetUserProfile(userProfileId);
            if (user == null)
            {
                return NotFound();
            }
            var pendingConnections = _connectionRepository.GetPendingConnections(userProfileId).ToList();
            var pendingcCnnectionsDTO = _mapper.Map<List<ConnectionDto>>(pendingConnections);
            if (pendingcCnnectionsDTO == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pendingcCnnectionsDTO);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateConnection([FromBody] ConnectionDto connectionCreate)
        {
            if (connectionCreate == null)
            {
                return BadRequest(ModelState);
            }

            var sender = _userProfileRepository.GetUserProfile(connectionCreate.SenderId);
            var receiver = _userProfileRepository.GetUserProfile(connectionCreate.ReceiverId);
            if (sender == null || receiver == null)
            {
                return BadRequest("Invalid user ID.");
            }
            var connectionExists = _connectionRepository.GetConnectionOfAUser(sender.Id);
            if(connectionExists.Any(c=> c.SenderId == receiver.Id || c.ReceiverId == receiver.Id))
            {
                ModelState.AddModelError("", "Connection already exists");
                return StatusCode(422, ModelState);
            }

            var newConnection = _mapper.Map<Connection>(connectionCreate);
            newConnection.Sender = sender;
            newConnection.Receiver = receiver;
            newConnection.Status = Connection.ConnectionStatus.Accepted;
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_connectionRepository.CreateConnection(newConnection))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(connectionCreate);
        }


        [HttpPut("{connectionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult AcceptConnection(int connectionId, [FromBody] ConnectionDto updatedConnection)
        {
            if(updatedConnection == null)
            {
                return BadRequest(ModelState);
            }
            if(connectionId != updatedConnection.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_connectionRepository.ConnectionExists(connectionId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var connectionToUpdate = _mapper.Map<Connection>(updatedConnection);


            if (!_connectionRepository.UpdateConnection(connectionToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating connection");
            }

            return NoContent();
        }



        [HttpDelete("{connectionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteConnection(int connectionId)
        {
            if (!_connectionRepository.ConnectionExists(connectionId))
            {
                return NotFound();
            }

            var connectionToDelete = _connectionRepository.GetConnection(connectionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_connectionRepository.DeleteConnection(connectionToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting connection");
            }

            return NoContent();
        }
    }

    
}
