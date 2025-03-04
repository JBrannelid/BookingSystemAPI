using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystemAPI.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan BookingTime { get; set; }

        [StringLength(500)]
        public string Description { get; set; }



        // Foreign Keys
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }


        // Navigation property to Customer/Employee object
        // Single references - one to many relationship
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
    }
}