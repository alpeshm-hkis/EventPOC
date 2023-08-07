using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class Comments
    {
        public int CommentId { get; set; }

        public int EventId { get; set; }

        public string Description { get;set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat]
        public DateTime Date { get; set;}

        public int UserId { get; set; }
        public string Author { get; set; }
    }
}
