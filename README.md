# Hospital Supply Chain Management System (HSCMS)

## Week 13 – Diagnostics and Health Checks  
**COP2839 – ASP.NET Development**  
**Taneisha Milkunic**  
**November 2025**

This week I implemented the Diagnostics feature for the Hospital Supply Chain Management System by adding a dedicated `/healthz` endpoint along with a real dependency check on the database. The purpose of this feature is to give the application a lightweight way for developers, administrators, and future deployment environments to quickly verify the health of the system without fully loading the UI or logging into the application.

Inside `Program.cs`, I registered ASP.NET Core Health Checks using `builder.Services.AddHealthChecks()` and then connected a real dependency check using a custom class called `DatabaseHealthCheck`. This class receives an instance of `ApplicationDbContext` and calls `Database.CanConnectAsync()` to validate that the application can successfully talk to the SQL Server database. If the database is online and reachable, the health check returns `Healthy`; otherwise, it returns `Unhealthy` with a high-level message to help identify the issue. No secrets, connection strings, or sensitive information are exposed in the output.

Next, I implemented the `/healthz` endpoint using `app.MapHealthChecks("/healthz", new HealthCheckOptions { ... })`. I created a custom `ResponseWriter` that returns a formatted JSON output containing the overall application status, each health check’s result, a description, and duration timings. This ensures the diagnostics are readable and useful for troubleshooting, and appropriate for container orchestrators, cloud services, or uptime monitoring tools.

Having a dedicated health endpoint is extremely important in real-world hospital supply chain environments where communication between software systems must be reliable. If the database ever goes down, or if there is a configuration or deployment issue, `/healthz` will immediately begin reporting degraded or failed status, allowing staff to take action quickly. This assignment demonstrates how proper diagnostics contribute to dependable software and prepares the project for scalable deployments in the future.

### Evidence Included
- Updated `Program.cs` showing Health Check registration and `/healthz` endpoint  
- Added `DatabaseHealthCheck.cs` for real DB dependency monitoring  
- Screenshot of the `/healthz` endpoint returning JSON  
- GitHub repository updated with Week 13 code changes  

Repository link:  
https://github.com/tmilkunic-lab/HospitalSupplyChainManagementSystem


# 🏥 Hospital Supply Chain Management System  
## Week 12 – CRUD (Vendors Vertical Slice)
# COP2839 ASP.NET Program w/C#
# Instructor: Franklin Castillo 
# Taneisha Milkunic
# November 2025

**Overview:**  
This week’s task was to implement a complete CRUD feature using ASP.NET Core MVC and EF Core with asynchronous operations and validation.

**What was implemented:**  
- Added `Vendor` model with `[Required]`, `[EmailAddress]`, and `[Phone]` attributes.  
- Registered the `DbSet<Vendor>` in `ApplicationDbContext`.  
- Generated `VendorsController` and Razor Views with full async support:
  - `ToListAsync()` for listing data  
  - `FindAsync()` / `FirstOrDefaultAsync()` for detail retrieval  
  - `SaveChangesAsync()` for create/edit/delete  
- Configured validation feedback in Create/Edit views.  
- Verified environment-specific configuration and connection string using LocalDB.  

**Evidence:**  
Screenshots in file folder



# 🏥 Hospital Supply Chain Management System  
## Week 11 – Separation of Concerns & Dependency Injection
# COP2839 ASP.NET Program w/C#
# Instructor: Franklin Castillo 
# Taneisha Milkunic
# November 2025

## Week 11 — Separation of Concerns & Dependency Injection

This week I refactored the Hospital Supply Chain Management System (ASP.NET Core MVC, .NET 8) to follow Separation of Concerns and use Dependency Injection for non-UI logic. I introduced a service layer that centralizes business rules and data access coordination so controllers remain thin and focused on HTTP concerns.

### What I implemented
- **Service contract** `IInventoryService` with `GetDashboardSummaryAsync()`.
- **Concrete service** `InventoryService` that uses `ApplicationDbContext` to compute a dashboard summary (products, suppliers, and order status counts).
- **DI registration** in `Program.cs` using `AddScoped<IInventoryService, InventoryService>()` (Scoped is appropriate for EF Core–backed services).
- **Controller injection** in `HomeController` and an async `Index` action that calls the service and returns a `DashboardSummary` model to the view.
- **View update** (`Views/Home/Index.cshtml`) strongly typed to `DashboardSummary`.

### Why this matters
Moving logic from controllers into services improves testability (mock the service), reuse (share across controllers), and maintainability (clearer responsibilities). DI decouples the interface from its implementation and makes dependencies explicit.

### Files
- `/Services/IInventoryService.cs`
- `/Services/InventoryService.cs`
- `/Models/DashboardSummary.cs`
- `/Controllers/HomeController.cs` (constructor + Index)
- `/Views/Home/Index.cshtml`
- `/Program.cs` (DI registration)

### Screenshots
Include: Solution Explorer showing `Services/`, DI registration in `Program.cs`, `HomeController` constructor with `IInventoryService`, and the running dashboard page.




# 🏥 Hospital Supply Chain Management System  
## Week 10 – Modeling

# COP2839 ASP.NET Program w/C#
# Instructor: Franklin Castillo 
# Taneisha Milkunic
# October 2025

### **Feature Implemented:**  
Creation of core data models for the Hospital Supply Chain Management System (HSCMS).

### **Objective:**  
To design the foundational database structure representing hospital supply chain entities (Suppliers, Products, and Orders) and establish relationships among them using Entity Framework Core in ASP.NET Core MVC.

---

### **Implementation Summary**

In Week 10, the modeling phase was completed by defining three primary entities: **Supplier**, **Product**, and **Order**, representing the key components of the hospital’s supply chain.  
Each class was defined in the `Models` folder with properties corresponding to essential database fields such as product name, quantity, supplier contact information, and order details.  

Following screenshot presents each individual class:

Supplier.cs <br>

![Supplier class](Supplier-class.png)

Product.cs <br>
![Product class](Product-class.png)

Order.cs <br>

![Order class](Order-class.png)


These entities were registered in the **`ApplicationDbContext`** class using `DbSet<T>` properties, allowing Entity Framework Core to generate corresponding database tables.  
Relationships were configured within the `OnModelCreating()` method to manage one-to-many associations between Suppliers and Products, and between Suppliers and Orders.  

A crucial design decision was to apply `.OnDelete(DeleteBehavior.Restrict)` for the `Order → Supplier` relationship to prevent multiple cascade paths, while maintaining `.OnDelete(DeleteBehavior.Cascade)` for `Product → Supplier`.  
This ensures that deleting a supplier automatically removes their products but restricts deletion when dependent orders exist.  

`ApplicationDbContext` showing Fluent API configuration with `OnDelete(DeleteBehavior.Restrict)`

![ApplicationDbContext](ApplicationDbContext.png)

After defining the models, the migration command `Add-Migration InitialCreate` was executed followed by `Update-Database`, successfully creating the **HSCMSDB** database with all related tables.  
Verification was performed through **SQL Server Object Explorer**, confirming the tables’ creation and relationships.

![Db screenshot](image-4.png)

---






