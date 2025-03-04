using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        // Navigation property
        public List<Booking> Bookings { get; set; }

    }
}