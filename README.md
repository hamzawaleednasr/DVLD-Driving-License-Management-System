# DVLD - Driving & Vehicle Licensing Department System

An enterprise-grade **Driving & Vehicle Licensing Department (DVLD)** desktop application built using the **N-Tier Architecture** pattern in **C# / .NET / WinForms**. It manages driving licenses, driver profiles, license test appointments, renewals, replacements, and license detentions.

---

## 🏗️ Architecture & Project Structure

The project is structured into four distinct layers to ensure modularity, separation of concerns, and clean coding practices:

1. **DVLD.Core**: Contains standard entity definitions, shared Data Transfer Objects (DTOs), ViewModels, and central Enums used across all layers.
2. **DVLD.DAL (Data Access Layer)**: Responsible for communicating directly with the MS SQL database using ADO.NET. It contains Repository implementations, SQL command mappings, and Database interfaces.
3. **DVLD.BLL (Business Logic Layer)**: Coordinates business rules, validation logic, calculations (e.g., license fees, expiration calculations), and coordinates transactions between the presentation layer and the database.
4. **DVLD.PL (Presentation Layer)**: The graphical user interface built with Windows Forms. It contains administrative forms, user controls, search filters, and application views.

---

## 🚀 Key Features

- **People & Users Management**: Centralized records of personal info (IDs, national numbers, photos) and login management (users, active/inactive statuses, roles).
- **Driving License Issuance**: End-to-end flow for applying, passing tests (Vision, Written, Practical), and issuing local licenses.
- **International Licenses**: Checking prerequisites, issuing international driving permits, and history tracking.
- **License Renewal**: Automatic validation of expiration states, deactivation of old licenses, and generation of renewed ones.
- **Replacements**: Easy processing for lost or damaged licenses.
- **License Detain & Release**: Fully fledged system to detain active licenses, record fine fees, and process payments/applications to release them.

---

## 🛠️ Technologies Used

- **Language**: C#
- **Framework**: .NET Framework (legacy format)
- **Database**: Microsoft SQL Server
- **UI Toolkit**: Windows Forms (WinForms)
- **Data Access**: ADO.NET (Raw SQL query optimization, parameterized commands)

---

## ⚙️ How to Run

1. Make sure you have **Microsoft SQL Server** installed locally.
2. Run the SQL script (located under `/schema` or provided in the database setup) to create the `DVLD` database structure.
3. Open `App.config` inside the `DVLD.PL` directory and configure the database connection string:
   ```xml
   <connectionStrings>
       <add name="DVLDConnectionString" connectionString="Data Source=YOUR_SERVER;Initial Catalog=DVLD;Integrated Security=True;" />
   </connectionStrings>
   ```
4. Open the solution in Visual Studio and run the `DVLD.PL` project.
