using BookingSystemAPI.DTOs.CustomerDTO;
using BookingSystemAPI.DTOs.EmployeesDTO;
using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.DTOs.BookingDTO
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        [MinLength(5)]
        [MaxLength(500)]
        public string Description { get; set; }


        // Include Customer and Employee
        public CustomerResponseDto Customer { get; set; }

        public EmployeeResponseDto Employee { get; set; }
    }
}