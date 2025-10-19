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



