# 💇‍♀️ Booking System API

![Booking System API](https://plus.unsplash.com/premium_photo-1661963874418-df1110ee39c1?w=900&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MXx8Y29kZXxlbnwwfHwwfHx8MA%3D%3D)

## 📋 About the Project

This is a school project from Chas Academy's Fullstack .NET Developer program. The project is a booking system API built with ASP.NET Core Minimal API that manages bookings, customers, and employees for a hair salon.

## 📢 Background

Klipp & Style Salon is a hair salon in a medium-sized city looking to modernize their booking system. Currently, they handle bookings via phone and a paper calendar, which often leads to double bookings and misunderstandings.

The salon has asked **DevSolve AB** to develop a backend solution where they can manage their appointments digitally. This API is the solution to that request.

## ✨ Core Features

- **Manage Bookings**: Create, view, update, and delete hair salon appointments
- **View Booked Times**: Stylists can retrieve a list of all booked appointments
- **Cancel Appointments**: Remove bookings when customers cancel

Each booking includes:
- 📅 Date (YYYY-MM-DD)
- ⏰ Time (HH:MM)
- 💈 Stylist name
- 👤 Customer name
- 📱 Customer phone number
- 📝 Description of the service

## 🛠️ Technologies

- ASP.NET Core Minimal API
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI

## 🔄 API Structure

The API consists of three main components:

### 📅 Bookings
- Get all bookings
- Get booking by ID
- Create new booking
- Update booking
- Delete booking
- Get bookings for a specific date

### 👥 Customers
- Get all customers
- Get customer by ID
- Create new customer
- Update customer
- Delete customer

### 💈 Employees (Stylists)
- Get all employees
- Get employee by ID
- Create new employee
- Update employee
- Delete employee

## 🚀 Getting Started

1. Clone the project
2. Configure your database connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookingSystemDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;"
  }
}
```
3. Run the following commands:

```bash
# Create the database
add-migration "init"
database-update

# Start the application
dotnet run
```

## 🔐 API Key Authentication

The API is protected with an API key:

1. Generate a new GUID and add it to appsettings.json as "ApiKey":
```json
{
  "ApiKey": "00000000-0000-0000-0000-000000000000"
}
```

2. Include the key in your request header for every API call:
```
X-Api-Key: your-guid-api-key
```

Without a valid API key, all requests will be rejected with a 401 Unauthorized status code.

## 📂 Project Structure

- **Models**: Data models for bookings, customers, and employees
- **DTOs**: Data Transfer Objects for safe data transfer
- **Endpoints**: Minimal API endpoints for each entity
- **Data**: DbContext for database connectivity
- **Middleware**: API key authentication

## 🎯 Learning Objectives

Through this project, I have:
- Created a RESTful API with ASP.NET Core Minimal API
- Implemented EF Core for data access
- Used the DTO pattern to separate application logic from data models
- Implemented basic authentication with an API key
- Used Swagger or Postman for API documentation

## 👨‍💻 Developed By

- Student at Chas Academy
- Fullstack .NET Developer program 2024