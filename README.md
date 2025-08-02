# Invoice Management System

A complete ASP.NET MVC application for managing invoices and their related items efficiently. This system provides user-friendly interfaces for handling invoice creation, editing, viewing, and item manipulation with dynamic interaction using AJAX.

---

## 🚀 Features

- ✅ Create, update, and delete invoices
- 🧾 Manage invoice items dynamically within the same page (edit/delete via AJAX)
- 📅 Assign invoices to cashiers and branches
- 💰 Real-time total price calculation
- ✨ Responsive UI with Bootstrap 5 and custom styles
- ⚙️ Entity Framework Core with code-first migrations
- 🧩 Clean MVC architecture (Model, View, Controller)

---

## 🛠️ Tech Stack

- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap 5**
- **Bootstrap Icons**
- **jQuery + AJAX**
- **C#**


## 🧱 Project Structure

├── Controllers/
│ └── InvoicesController.cs
├── Models/
│ ├── InvoiceHeader.cs
│ └── InvoiceDetail.cs
---

├── Views/
│ └── Invoices/
│ ├── Index.cshtml
│ ├── Create.cshtml
│ ├── Edit.cshtml
│ ├── Details.cshtml
---

├── Migrations/
├── wwwroot/
├── appsettings.json
└── Startup.cs
---

## 🧪 How to Run the Project Locally

1. **Clone the repository**  
   ```bash
   git clone https://github.com/your-username/InvoiceManagementSystem.git

2. Open the solution in Visual Studio

3. Update the connection string in appsettings.json to your local SQL Server instance.

4. Apply migrations
Add-Migration InitialCreate
Update-Database

5. Run the application
dotnet run

6. Open your browser and navigate to:
   https://localhost:5001/Invoices
   
## 🤝 Author
Fatma Elshihna
.NET Full Stack Developer – ITI Trainee

🔗 LinkedIn Profile:https://www.linkedin.com/in/fatma-elshihna/












