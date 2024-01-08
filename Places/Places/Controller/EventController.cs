using AutoMapper;
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
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public EventController(IEventRepository eventRepository, ILocationRepository locationRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Event>))]
        public IActionResult GetEvents()
        {
            var events = _mapper.Map<List<EventDto>>(_eventRepository.GetEvents());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(events);
        }

        [HttpGet("{eventId}")]
        [ProducesResponseType(200, Type = typeof(Event))]
        public IActionResult GetEvent(int eventId)
        {
            if(!_eventRepository.EventExists(eventId))
                return NotFound();
            
            var eventById = _mapper.Map<EventDto>(_eventRepository.GetEvent(eventId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(eventById);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateEvent([FromQuery] int locationId, [FromBody] EventDto createdEvent)
        {
            if (createdEvent == null)
                return BadRequest(ModelState);

            var eventExists = _eventRepository.GetEvents().Where(e => e.Id == createdEvent.Id).FirstOrDefault();
            if(eventExists != null)
            {
                ModelState.AddModelError("", "Event already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var eventMap = _mapper.Map<Event>(createdEvent);
            eventMap.EventLocationId = locationId;
            eventMap.EventLocation = _locationRepository.GetLocation(locationId);

            if (!_eventRepository.CreateEvent(eventMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
