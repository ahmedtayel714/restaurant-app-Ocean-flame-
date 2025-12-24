# Restaurant Management System

## Overview
This is a Restaurant Management System built with **ASP.NET Core 6+**. It features a public-facing website for customers to view the menu, make reservations, and place orders, as well as an Admin Dashboard for managing menu items, viewing orders, and handling reservations.

## Getting Started

### Prerequisites
- .NET 6.0 SDK or later
- A code editor (VS Code, Visual Studio)

### How to Run
1. Navigate to the project directory:
   ```bash
   cd Restuarant
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. Open your browser and navigate to `http://localhost:5000`.

## Project Structure & File Explanation

### Root Directory
- **`Program.cs`**: The entry point of the application.
  - Configures the dependency injection container (services).
  - Sets up the database context (`RestaurantContext`) with SQLite.
  - Configures Session state for Admin authentication.
  - Seeds the database with initial menu items if empty.
  - Configures the HTTP request pipeline (Middleware) including Static Files, Routing, Session, and Endpoint mapping for Razor Pages and Controllers.
- **`appsettings.json`**: Configuration file containing the database connection string and logging settings.

### Data Layer (`/Data`)
- **`RestaurantContext.cs`**: The Entity Framework Core database context.
  - Inherits from `DbContext`.
  - Defines `DbSet` properties for `Orders`, `OrderItems`, `Reservations`, and `MenuItems`, representing tables in the SQLite database.

### Models (`/Models`)
These classes represent the data structure of the application.
- **`MenuItem.cs`**: Represents a dish on the menu. Contains properties like `Name`, `Description`, `Price`, `StockQuantity`, `ImageUrl`, and `Category`.
- **`Order.cs`**: Represents a customer order. Contains a list of `OrderItem`s, `TotalPrice`, `CreatedAt` timestamp, and `Status`.
- **`OrderItem.cs`**: Represents a specific item within an order (e.g., "2x Burger"). Links back to an `Order`.
- **`Reservation.cs`**: Represents a table reservation. Contains customer details (`Name`, `Email`, `Phone`), `People` count, `Message`, `CreatedAt`, and `Status`.

### Controllers (`/controllers`)
API Controllers handle HTTP requests from the client-side JavaScript (AJAX/Fetch).
- **`OrderController.cs`**:
  - `POST /api/Order/confirm`: Receives a list of order items, validates stock, calculates total, deducts stock, and saves the order to the database.
  - `GET /api/Order/all`: Returns all orders (used by Admin).
- **`ReservationController.cs`**:
  - `POST /api/Reservation/book`: Receives reservation details and saves them to the database.
  - `GET /api/Reservation/all`: Returns all reservations (used by Admin).

### Pages (`/Pages`)
Razor Pages for server-side rendering of HTML.
- **`Index.cshtml` / `Index.cshtml.cs`**: The main landing page.
  - Displays the Menu (fetched from database).
  - Contains the "Book a Table" form.
  - Contains the "Order" modal and cart logic (handled via JavaScript in `site.js` interacting with APIs).
- **`Admin/` Directory**: Protected pages for restaurant staff.
  - **`Login.cshtml`**: Admin login page.
  - **`Menu.cshtml`**: Dashboard to View, Add, Edit, and Delete menu items. Handles stock updates.
  - **`Orders.cshtml`**: View incoming customer orders.
  - **`Reservations.cshtml`**: View table reservations.

### Static Files (`/wwwroot`)
- **`css/`**: Stylesheets for the application.
- **`js/`**: JavaScript files.
  - `site.js` (assumed): Likely contains logic for the Shopping Cart, "Add to Cart" buttons, and calling the `OrderController` API.
- **`img/`**: Images for menu items and site assets.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Order/confirm` | Submit a new order. Body: `List<OrderItem>` |
| GET | `/api/Order/all` | Get all orders history. |
| POST | `/api/Reservation/book` | Submit a new reservation. Body: `Reservation` |
| GET | `/api/Reservation/all` | Get all reservations. |

## Authentication
- **Admin Access**: Uses Session-based authentication.
- **Credentials**: Hardcoded check in `Login.cshtml.cs` (e.g., `Ahmedtayel714@gmail.com` / `Ahmed.101`).

## Database
- Uses **SQLite** (`restaurant.db`).
- Created automatically on application startup if it doesn't exist (`context.Database.EnsureCreated()`).