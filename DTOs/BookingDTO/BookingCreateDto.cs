using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.DTOs.BookingDTO
{
    public class BookingCreateDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan BookingTime { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        public string Description { get; set; }

        // A booking is reference by a customer and employeer ID

        public int CustomerId { get; set; }

        public int EmployeeId { get; set; }
    }
}