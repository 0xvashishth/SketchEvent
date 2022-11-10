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
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"\d{10}", ErrorMessage = "Please enter 10 digit Mobile No.")]
        public string PhoneNo { get; set; }
        public string? Token { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
