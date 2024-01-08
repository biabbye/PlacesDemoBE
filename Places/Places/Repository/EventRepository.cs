using AutoMapper;
using Places.Data;
using Places.Interfaces;
using Places.Models;

namespace Places.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly PlacesContext _context;
        private readonly IMapper _mapper;

        public EventRepository(PlacesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateEvent(Event createdEvent)
        {
            _context.Events.Add(createdEvent);
            return Save();
        }

        public bool DeleteEvent(Event deletedEvent)
        {
            _context.Events.Remove(deletedEvent);
            return Save();
        }

        public bool EventExists(int eventId)
        {
            return _context.Events.Any(e => e.Id == eventId);
        }

        public Event GetEvent(int eventId)
        {
            return _context.Events.Where(e => e.Id == eventId).FirstOrDefault();
        }

        public ICollection<Event> GetEvents()
        {
            return _context.Events.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateEvent(Event updatedEvent)
        {
            _context.Events.Update(updatedEvent);
            return Save();
        }
    }
}
