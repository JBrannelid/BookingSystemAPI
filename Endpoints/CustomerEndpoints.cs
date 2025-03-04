using BookingSystemAPI.Data;
using BookingSystemAPI.DTOs.CustomerDTO;
using BookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Endpoints
{
    public class CustomerEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // ---------- GET all Customers ----------------------------- //
            app.MapGet("/api/customers", async (AppDbContext dBcontext) =>
            {
                // Respons in DTOs Customers
                var CustomerList = await dBcontext.Customers.Select(c => new CustomerResponseDto
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Number = c.Number,
                    EmailAddress = c.EmailAddress
                }).ToListAsync();

                return Results.Ok(CustomerList);  // Statuscode - 200 Ok
            });

            // ---------- Get a specifik customer ID dynamic ------------ //
            app.MapGet("/api/customers/{id}", async (AppDbContext dBcontext, int id) =>
            {
                // Find customer with specific Id
                var selectedCustomer = await dBcontext.Customers.Select(c => new CustomerResponseDto
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Number = c.Number,
                    EmailAddress = c.EmailAddress
                }).FirstOrDefaultAsync(c => c.CustomerId == id); 

                // If no customer found, display error code
                if (selectedCustomer == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                return Results.Ok(selectedCustomer); // Statuscode - 200 Ok
            });

            // ---------- Create new customer --------------------------- //
            app.MapPost("/api/customers", async (AppDbContext dBcontext, CustomerDto newCustomer) =>
            {
                // 1. Validate context
                var validationContext = new ValidationContext(newCustomer);
                var validationResult = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(newCustomer, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage)); // Statuscode - 400 Bad request
                }

                // 2. Create a new Customer object from DTOs
                var customer = new Customer
                {
                    FirstName = newCustomer.FirstName,
                    LastName = newCustomer.LastName,
                    Number = newCustomer.Number,
                    EmailAddress = newCustomer.EmailAddress
                };

                // 3. Add object to database
                dBcontext.Customers.Add(customer);
                await dBcontext.SaveChangesAsync();
                return Results.Created($"/api/employees/{customer.CustomerId}", customer); // Statuscode - 201 Created
            });

            // ---------- Update Customer ------------------------------ //
            app.MapPut("/api/customers/{id}", async (AppDbContext dBcontext, int id, CustomerDto updateCustomer) =>
            {
                // 1. Find Employee ID dynamic with first or default
                var existingCustomer = await dBcontext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

                if (existingCustomer == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                // 2. Update customer information 
                existingCustomer.FirstName = updateCustomer.FirstName;
                existingCustomer.LastName = updateCustomer.LastName;
                existingCustomer.Number = updateCustomer.Number;
                existingCustomer.EmailAddress = updateCustomer.EmailAddress;

                // 3. Save changes to dbContext
                await dBcontext.SaveChangesAsync();

                // 4. Return
                return Results.Ok(); // Statuscode - 200 Ok
            });

            // ---------- Delete an customer --------------------------- //
            app.MapDelete("/api/customers/{id}", async (AppDbContext dBcontext, int id) =>
            {
                var existingCustomer = await dBcontext.Customers.FindAsync(id);
                if (existingCustomer == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                dBcontext.Customers.Remove(existingCustomer);
                await dBcontext.SaveChangesAsync();

                return Results.NoContent(); // Statuscode - 204 No Content
            });
        }
    }
}