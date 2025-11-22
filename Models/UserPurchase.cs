using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeService.Models
{
    public class UserPurchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string MovieId { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    }
}
