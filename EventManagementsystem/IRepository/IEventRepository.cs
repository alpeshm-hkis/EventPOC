using EventManagementSystem.Models;

namespace EventManagementSystem.IRepository
{
    public interface IEventRepository
    {
        void SaveEventDetails(EventDetails eventDetails);

        List<EventDetails> GetEventDetailsList(int isPublic, int type, int userId);
        List<Comments> GetEventComments(string id);
        void AddEventComments(string id, string comment);
        void DeleteEvent(int id);
        EventDetails GetEventDetailById(int eventId);
    }
}
