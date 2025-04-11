# Orphanage Management System - ASP.NET MVC
## 📌 Overview
A web-based management system for orphanages, built with **ASP.NET MVC**. This system helps orphanage administrators manage daily operations efficiently, including child profiles, health checkups, sponsorship tracking, adoption processes, and expense management.

## ⚙️ Features

- 👶 **Child Management**: Add, update, and view detailed profiles of orphans.
- 🩺 **Health Records**: Schedule and manage health checkups, track height and weight.
- 💊 **Medicine Management**: Track available medicines and prescriptions.
- 🧾 **Expense Management**: Record daily expenses using sponsor funds.
- 🤝 **Adoption Process**: Handle adoption requests and approvals.
- 💝 **Sponsor Tracking**: Manage sponsor details and donations.
- 📊 **Dashboard**: Visual charts and statistics for health and finances.

## 🛠️ Technologies

- ASP.NET MVC 5
- Entity Framework
- MS SQL Server
- Bootstrap 5 / Tailwind CSS
- Chart.js / DataTables
## Not Yet Implemented
During the development process, our team realized that directly deleting child records (DELETE) from the database could lead to the loss of important information such as medical history, health records, or links to other tables (e.g., sponsorships, adoptions, etc.).
Therefore, we intended to apply a Soft Delete mechanism by adding an IsDeleted column to the Tre (Children) table. Instead of permanently removing a record, the system would simply update IsDeleted = 1.
However, this functionality has not yet been implemented in the current version of the system. All deletions are still hard deletes, which may affect data integrity. We plan to integrate Soft Delete in future versions to ensure safer data handling and better support for data recovery and historical statistics.


