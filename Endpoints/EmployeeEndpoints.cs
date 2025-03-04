using BookingSystemAPI.Data;
using BookingSystemAPI.DTOs.EmployeesDTO;
using BookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Endpoints
{
    public class EmployeeEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // ---------- GET all Employees ---------------------------- //

            app.MapGet("/api/employees", async (AppDbContext dBcontext) =>
            {
                // Respons in DTOs Employee
                var employeeList = await dBcontext.Employees.Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                }).ToListAsync();

                return Results.Ok(employeeList);  // Statuscode - 200 Ok
            });

            // ---------- Get a specifik employees ID dynamic ---------- //
            app.MapGet("/api/employees/{id}", async (AppDbContext dBcontext, int id) =>
            {
                var selectedEmployee = await dBcontext.Employees.Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                }).FirstOrDefaultAsync(e => e.EmployeeId == id); 

                // If no employee found, display error code
                if (selectedEmployee == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                return Results.Ok(selectedEmployee); // Statuscode - 200 Ok
            });

            // ---------- Create new Employee -------------------------- //
            app.MapPost("/api/employees", async (AppDbContext dBcontext, EmployeeDto newEmployee) =>
            {
                // 1. Validate context
                var validationContext = new ValidationContext(newEmployee);
                var validationResult = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(newEmployee, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage)); // Statuscode - 400 Bad request
                }

                // 2. Create a new employee object from DTOs
                var employee = new Employee
                {
                    FirstName = newEmployee.FirstName,
                    LastName = newEmployee.LastName
                };

                // 3. Add object to database
                dBcontext.Employees.Add(employee);
                await dBcontext.SaveChangesAsync();
                return Results.Created($"/api/employees/{employee.EmployeeId}", employee); // Statuscode - 201 Created
            });

            // ---------- Update Employee ----------------------------- //
            app.MapPut("/api/employees/{id}", async (AppDbContext dBcontext, int id, EmployeeDto updateEmployee) =>
            {
                // 1. Find Employee ID dynamic with first or default
                var existingEmployee = await dBcontext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (existingEmployee == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                // 2. Update excisting Employee
                existingEmployee.FirstName = updateEmployee.FirstName;
                existingEmployee.LastName = updateEmployee.LastName;

                // 3. Save changes to dbContext
                await dBcontext.SaveChangesAsync();

                // 4. Return
                return Results.Ok(); // Statuscode - 200 Ok
            });

            // ---------- Delete an Employee -------------------------- //
            app.MapDelete("/api/employees/{id}", async (AppDbContext dBcontext, int id) =>
            {
                // 1. Find Employee by Id
                var existingEmployee = await dBcontext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (existingEmployee == null)
                {
                    return Results.NoContent(); // Statuscode - 204 No Content
                }

                // 2. Remove Employee from the database and save changes
                dBcontext.Employees.Remove(existingEmployee);
                await dBcontext.SaveChangesAsync();

                // 3. Return
                return Results.Ok(); // Statuscode - 200 Ok
            });
        }
    }
}