using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventoWeb.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Venue { get; set; }
        public string? AdditionalNote { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }
        public ICollection<User>? Registries { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
