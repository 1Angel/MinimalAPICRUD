using System.ComponentModel.DataAnnotations;

namespace minimalapiCRUD
{
    public class Post
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createdDate { get; set; } = DateTime.UtcNow;
    }
}
