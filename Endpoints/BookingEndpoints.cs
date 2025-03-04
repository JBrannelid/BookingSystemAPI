using Azure;
using BookingSystemAPI.Data;
using BookingSystemAPI.DTOs.BookingDTO;
using BookingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI.Endpoints
{
    public class BookingEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // ---------- GET all Bookings with pagination ------------- //
            app.MapGet("/api/bookings", async (AppDbContext dBcontext, int page = 1, int pageSize = 10) =>
            {
                // Validate pagination parameters
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 50) pageSize = 50;

                // Calculate skip count for pagination
                int skip = (page - 1) * pageSize;

                // Get total count for pagination metadata
                var totalCount = await dBcontext.Bookings.CountAsync();

                // Get paginated bookings with related Customer and Employee data
                // Skip and take is the core functionality of pagination!
                var bookings = await dBcontext.Bookings
                     .Include(b => b.Customer)
                     .Include(b => b.Employee)
                     .Skip(skip)
                     .Take(pageSize)
                     .ToListAsync();

                // Map to DTOs to avoid circular references
                var bookingDtos = bookings.Select(b => new BookingResponseDto
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    BookingTime = b.BookingTime,
                    Description = b.Description,
                    Customer = new DTOs.CustomerDTO.CustomerResponseDto
                    {
                        CustomerId = b.Customer.CustomerId,
                        FirstName = b.Customer.FirstName,
                        LastName = b.Customer.LastName,
                        Number = b.Customer.Number,
                        EmailAdress = b.Customer.EmailAdress
                    },
                    Employee = new DTOs.EmployeesDTO.EmployeeResponseDto
                    {
                        EmployeeId = b.Employee.EmployeeId,
                        FirstName = b.Employee.FirstName,
                        LastName = b.Employee.LastName
                    }
                }).ToList();

                // Create object with result and pagination  
                return Results.Ok(new
                {
                    Bookings = bookingDtos,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                });  // Statuscode - 200 Ok
            });

            // ---------- GET booking by ID ----------------- //
            app.MapGet("/api/bookings/{id}", async (AppDbContext dBcontext, int id) =>
            {
                // 1. Find booking with related data
                var booking = await dBcontext.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Employee)
                    .FirstOrDefaultAsync(b => b.BookingId == id);

                // 2. Return 404 if booking not found
                if (booking == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                // 3. Map to DTO
                var bookingDto = new BookingResponseDto
                {
                    BookingId = booking.BookingId,
                    BookingDate = booking.BookingDate,
                    BookingTime = booking.BookingTime,
                    Description = booking.Description,
                    Customer = new DTOs.CustomerDTO.CustomerResponseDto
                    {
                        CustomerId = booking.Customer.CustomerId,
                        FirstName = booking.Customer.FirstName,
                        LastName = booking.Customer.LastName,
                        Number = booking.Customer.Number,
                        EmailAdress = booking.Customer.EmailAdress
                    },
                    Employee = new DTOs.EmployeesDTO.EmployeeResponseDto
                    {
                        EmployeeId = booking.Employee.EmployeeId,
                        FirstName = booking.Employee.FirstName,
                        LastName = booking.Employee.LastName
                    }
                };

                // 4. Return booking
                return Results.Ok(bookingDto); // Statuscode - 200 Ok
            });

            // ---------- Create new booking ---------------- //
            app.MapPost("/api/bookings", async (AppDbContext dBcontext, BookingCreateDto newBooking) =>
            {
                // 1. Validate if customer and employee exists
                var customerExists = await dBcontext.Customers.AnyAsync(c => c.CustomerId == newBooking.CustomerId);
                if (!customerExists)
                {
                    return Results.BadRequest("Customer not found"); // Statuscode - 400 Bad Request
                }

                var employeeExists = await dBcontext.Employees.AnyAsync(e => e.EmployeeId == newBooking.EmployeeId);
                if (!employeeExists)
                {
                    return Results.BadRequest("Employee not found"); // Statuscode - 400 Bad Request
                }

                // 2. Create the new booking
                var booking = new Booking
                {
                    BookingDate = newBooking.BookingDate,
                    BookingTime = newBooking.BookingTime,
                    Description = newBooking.Description,
                    CustomerId = newBooking.CustomerId,
                    EmployeeId = newBooking.EmployeeId
                };

                // 4. Save to database
                dBcontext.Bookings.Add(booking);
                await dBcontext.SaveChangesAsync();

                return Results.Created($"/api/bookings/{booking.BookingId}", booking); // Statuscode - 201 Created
            });

            // ---------- Delete booking by ID -------------- //
            app.MapDelete("/api/bookings/{id}", async (AppDbContext dBcontext, int id) =>
            {
                // 1. Find booking to delete
                var booking = await dBcontext.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return Results.NoContent(); // Statuscode - 204 No Content
                }

                // 2. Remove booking
                dBcontext.Bookings.Remove(booking);
                await dBcontext.SaveChangesAsync();

                // 3. Return
                return Results.NoContent(); // Statuscode - 204 No Content
            });

            // ---------- Update booking -------------------- //
            app.MapPut("/api/bookings/{id}", async (AppDbContext dBcontext, int id, BookingCreateDto updateBooking) =>
            {
                // 1. Find booking by ID
                var existingBooking = await dBcontext.Bookings.FindAsync(id);
                if (existingBooking == null)
                {
                    return Results.NotFound(); // Statuscode - 404 Not Found
                }

                // 2. Validate customer exists
                var customerExists = await dBcontext.Customers.AnyAsync(c => c.CustomerId == updateBooking.CustomerId);
                if (!customerExists)
                {
                    return Results.BadRequest("Customer not found"); // Statuscode - 400 Bad Request
                }

                // 3. Validate employee exists
                var employeeExists = await dBcontext.Employees.AnyAsync(e => e.EmployeeId == updateBooking.EmployeeId);
                if (!employeeExists)
                {
                    return Results.BadRequest("Employee not found"); // Statuscode - 400 Bad Request
                }

                // 4. Update booking information
                existingBooking.BookingDate = updateBooking.BookingDate;
                existingBooking.BookingTime = updateBooking.BookingTime;
                existingBooking.Description = updateBooking.Description;
                existingBooking.CustomerId = updateBooking.CustomerId;
                existingBooking.EmployeeId = updateBooking.EmployeeId;

                // 5. Save changes to database
                await dBcontext.SaveChangesAsync();

                // 6. Return
                return Results.Ok(); // Statuscode - 200 Ok
            });

            // ---------- GET bookings by date -------------- //
            app.MapGet("/api/bookings/date/{date}", async (AppDbContext dBcontext, DateTime date) =>
            {
                // 1. Get all bookings for this date with related data
                var dateBookings = await dBcontext.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Employee)
                    .Where(b => b.BookingDate.Date == date.Date)
                    .ToListAsync();

                // 2. Map to DTOs
                var bookingDtos = dateBookings.Select(b => new BookingResponseDto
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    BookingTime = b.BookingTime,
                    Description = b.Description,
                    Customer = new DTOs.CustomerDTO.CustomerResponseDto
                    {
                        CustomerId = b.Customer.CustomerId,
                        FirstName = b.Customer.FirstName,
                        LastName = b.Customer.LastName,
                        Number = b.Customer.Number,
                        EmailAdress = b.Customer.EmailAdress
                    },
                    Employee = new DTOs.EmployeesDTO.EmployeeResponseDto
                    {
                        EmployeeId = b.Employee.EmployeeId,
                        FirstName = b.Employee.FirstName,
                        LastName = b.Employee.LastName
                    }
                }).ToList();

                // 3. Return bookings
                return Results.Ok(bookingDtos); // Statuscode - 200 Ok
            });
        }
    }
}