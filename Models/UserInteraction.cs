using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeService.Models
{
    public class UserInteraction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string MovieId { get; set; }

        public string InteractionType { get; set; } // "View", "Like"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
