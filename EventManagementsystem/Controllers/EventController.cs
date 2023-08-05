using EventManagementSystem.IService;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementsystem.Controllers
{
    
    public class EventController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventDetailsService;
        public EventController(ILogger<HomeController> logger, IEventService eventDetailsService)
        {
            _logger = logger;
            _eventDetailsService = eventDetailsService;
        }
        public IActionResult Index()
        {
            // Add Event 
            return View();
        }
        public IActionResult UserEventList(string type)
        {
            var userId = HttpContext.Session.GetString("UserId");
            List<EventDetails> evenList = new List<EventDetails>();
            evenList = _eventDetailsService.GetEventDetailsList(0, Convert.ToInt32(type), Convert.ToInt32(userId));
            return View(evenList);
        }
        public IActionResult AddEvent(EventDetails eventData)
        {
            eventData.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            _eventDetailsService.SaveEventDetails(eventData);
            return Ok();
        }
        public IActionResult DeleteEvent(string id)
        {
            _eventDetailsService.DeleteEvent(Convert.ToInt32(id));
            var userId = HttpContext.Session.GetString("UserId");
            List<EventDetails> evenList = new List<EventDetails>();
            evenList = _eventDetailsService.GetEventDetailsList(0, Convert.ToInt32(0), Convert.ToInt32(userId));
            return View("UserEventList.cshtml", evenList);
        }
        public IActionResult Edit(string id)
        {
            EventDetails evenData = new EventDetails();
            evenData = _eventDetailsService.GetEventDetailById(Convert.ToInt32(id));
            return View(evenData);
        }
        public IActionResult GetEventById(string id)
        {
            EventDetails evenData = new EventDetails();
            evenData = _eventDetailsService.GetEventDetailById(Convert.ToInt32(id));
            return Ok(evenData);
        }
    }
}
