namespace EventoWeb.Models
{
    public interface IEventRepository
    {
        Event GetEventById(int id);
        Event Add(Event Event);
        Event Update(Event EventChanges);
        Event Delete(int id);
        IEnumerable<Event> GetAllEvents();
    }
}
