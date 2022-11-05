using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventoWeb.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string? Token { get; set; }
        [Required]
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
