﻿using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.DTOs.CustomerDTO
{
    public class CustomerDto
    {
        [MinLength(5)]
        [MaxLength(250)]
        public string FirstName { get; set; }

        [MinLength(5)]
        [MaxLength(250)]
        public string LastName { get; set; }

        [Phone]
        public string Number { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}