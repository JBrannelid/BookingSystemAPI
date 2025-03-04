using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Number { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        // Navigation property
        public List<Booking> Bookings { get; set; }

    }
}