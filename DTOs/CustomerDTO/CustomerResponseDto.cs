using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.DTOs.CustomerDTO
{
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        [MinLength(5)]
        [MaxLength(250)]
        public string FirstName { get; set; }
        [MinLength(5)]
        [MaxLength(250)]
        public string LastName { get; set; }
        [Phone]
        public string Number { get; set; }
        [EmailAddress]
        public string EmailAdress { get; set; }
    }
}