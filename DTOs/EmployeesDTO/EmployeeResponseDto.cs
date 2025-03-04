using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.DTOs.EmployeesDTO
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        [MinLength(5)]
        [MaxLength(250)]
        public string FirstName { get; set; }
        [MinLength(5)]
        [MaxLength(250)]
        public string LastName { get; set; }
    }
}