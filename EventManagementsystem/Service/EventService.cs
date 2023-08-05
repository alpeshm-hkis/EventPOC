using EventManagementSystem.IRepository;
using EventManagementSystem.IService;
using EventManagementSystem.Models;

namespace EventManagementSystem.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventDetailRepository;

        public EventService(IEventRepository eventDetailRepository)
        {
            _eventDetailRepository = eventDetailRepository;
        }

        public List<EventDetails> GetEventDetailsList(int isPublic, int type, int userId)
        {
            List<EventDetails> events = _eventDetailRepository.GetEventDetailsList(isPublic, type, userId);
            return events;
        }

        public void SaveEventDetails(EventDetails eventDetails)
        {
            _eventDetailRepository.SaveEventDetails(eventDetails);
        }

        public List<Comments> GetEventComments(string id)
        {
            List<Comments> commentList = _eventDetailRepository.GetEventComments(id);
            return commentList;
        }

        public void AddEventComments(string id, string comment)
        {
            _eventDetailRepository.AddEventComments(id, comment);
        }

        public void DeleteEvent(int id)
        {
            _eventDetailRepository.DeleteEvent(id);
        }
        public EventDetails GetEventDetailById(int eventId)
        {
            EventDetails eventDetails = _eventDetailRepository.GetEventDetailById(eventId);
            return eventDetails;
        }
    }
}
