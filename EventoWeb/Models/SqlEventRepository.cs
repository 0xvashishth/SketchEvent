using EventoWeb.Data;

namespace EventoWeb.Models
{
    public class SqlEventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db;
        public SqlEventRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Event Add(Event Event)
        {
            _db.Events.Add(Event);
            _db.SaveChanges();
            return Event;
        }

        public Event Delete(int id)
        {
            Event Event = _db.Events.Find(id);
            if (Event != null)
            {
                _db.Events.Remove(Event);
                _db.SaveChanges();
            }
            return Event;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _db.Events;
        }

        public Event GetEventById(int id)
        {
            return _db.Events.FirstOrDefault(m => m.EventId == id);
        }

        public Event Update(Event EventChanges)
        {
            var Event = _db.Events.Attach(EventChanges);
            Event.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return EventChanges;
        }
    }
}
