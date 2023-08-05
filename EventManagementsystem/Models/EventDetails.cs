using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class EventDetails
    {
        public int EventId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get;set; }
        [Required(ErrorMessage = "StartDate is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime EndTime { get; set; }
        public string Duration { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string Location { get; set; }

        public int UserId { get; set; }

        public string Author { get; set; }

        public bool IsPublic { get; set; }
    }
}
