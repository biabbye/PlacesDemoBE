using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Places.Dto;
using Places.Interfaces;
using Places.Models;

namespace Places.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {

        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IMapper _mapper;
        public UserProfileController(IUserProfileRepository userProfileRepository, IConnectionRepository connectionRepository, ILocationRepository locationRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _locationRepository = locationRepository;
            _connectionRepository = connectionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserProfile>))]
        public IActionResult GetUserProfiles()
        {
            var userProfiles = _mapper.Map<List<UserProfileDto>>(_userProfileRepository.GetUserProfiles());
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfiles);
        }

        [HttpGet("{userProfileId}")]
        [ProducesResponseType(200, Type = typeof(UserProfile))]
        [ProducesResponseType(400)]
        public IActionResult GetUserProfile(int userProfileId)
        {
            if (!_userProfileRepository.UserProfileExists(userProfileId))
                return NotFound();

            var userProfile = _mapper.Map<UserProfileDto>(_userProfileRepository.GetUserProfile(userProfileId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfile);
        }
        [HttpGet("GetUserProfileByPhone/{phoneNumber}")]
        [ProducesResponseType(200, Type = typeof(UserProfile))]
        [ProducesResponseType(400)]
        public IActionResult GetUserProfileByPhone(string phoneNumber)
        {
           
            var userProfile = _mapper.Map<UserProfileDto>(_userProfileRepository.GetUserProfileByPhone(phoneNumber));

            if(userProfile == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfile);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserProfile([FromQuery] int locationId, [FromBody] UserProfileDto userProfileCreate)
        {
            if (userProfileCreate == null)
                return BadRequest(ModelState);

            var userProfileExist = _userProfileRepository.GetUserProfiles().Where(up => up.PhoneNumber == userProfileCreate.PhoneNumber || up.Email == userProfileCreate.Email).FirstOrDefault();

            if (userProfileExist != null)
            {
                ModelState.AddModelError("", "User Profile already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userProfileMap = _mapper.Map<UserProfile>(userProfileCreate);
            userProfileMap.CurrentLocationId = locationId;
            userProfileMap.UserLocation = _locationRepository.GetLocation(locationId);


            if (!_userProfileRepository.CreateUserProfile( userProfileMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{userProfileId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUserProfile(int userProfileId,
            [FromBody] UserProfileDto updatedUserProfile)
        {
            if (updatedUserProfile == null)
                return BadRequest(ModelState);

            if(userProfileId != updatedUserProfile.Id)
            {
                return BadRequest(ModelState);
            }

            //var userProfileToBeUpdated = _userProfileRepository.GetUserProfile(userProfileId);

            if (!_userProfileRepository.UserProfileExists(userProfileId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var userProfileMap = _mapper.Map<UserProfile>(updatedUserProfile);

            if (!_userProfileRepository.UpdateUserProfile(userProfileMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userProfileId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUserProfile(int userProfileId)
        {
            if (!_userProfileRepository.UserProfileExists(userProfileId))
            {
                return NotFound();
            }

            var userProfileToDelete = _userProfileRepository.GetUserProfile(userProfileId);
            var userConnections = _connectionRepository.GetConnectionOfAUser(userProfileId);

            if(userConnections!=null )
            {
                foreach (var userConnection in userConnections)
                {
                    _connectionRepository.DeleteConnection(userConnection);
                }
            }
          
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userProfileRepository.DeleteUserProfile(userProfileToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting user profile");
            }

            return NoContent();
        }
    }
}
