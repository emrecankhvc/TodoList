using System.ComponentModel.DataAnnotations.Schema;

namespace TodoProject.Entities
{
    public class UserNote
    {
        public int Id { get; set; }
        public string? Note { get; set; }


        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
