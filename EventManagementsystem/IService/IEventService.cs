using EventManagementSystem.Models;

namespace EventManagementSystem.IService
{
    public interface IEventService
    {
        void SaveEventDetails(EventDetails eventDetails);

        List<EventDetails> GetEventDetailsList(int isPublic, int type, int userId);
        List<Comments> GetEventComments(string id);
        void AddEventComments(string id, string comment, int userId);
        void DeleteEvent(int id);
        EventDetails GetEventDetailById(int eventId);
    }
}
