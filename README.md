# Support Ticket System with Real-time Notifications

This is a modern support ticketing system developed using **ASP.NET Core**, **SignalR**, and **Entity Framework Core**. It provides a chat-like interface for users and administrators to communicate in real-time about support tickets, with robust notification features.

---

## Features

- **User & Admin Roles:** Separate experiences for end-users and admins.
- **Ticket Management:** Users can create, view, and track tickets; admins can manage and respond.
- **Real-time Messaging:** Instant chat updates between users and admins via SignalR.
- **Notification System:** Read/unread notifications with counts, delivered instantly.
- **Responsive UI:** Clean and mobile-friendly interface using Bootstrap 5.
- **Role-based Notifications:** Notifications tailored for user or admin roles.
- **Pagination:** Efficient data loading on ticket and user lists.
- **Security:** Authentication and authorization integrated.

---

## Technologies Used

- ASP.NET Core 7+
- SignalR for real-time communication
- Entity Framework Core with SQL Server
- Bootstrap 5 for UI
- C#
- Toastr.js for notification popups

---

## Getting Started

### Prerequisites

- [.NET SDK 7+](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository

```bash
git clone https://github.com/yourusername/support-ticket-system.git
cd support-ticket-system

2. Configure the connection string in appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=SupportSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

3. Apply migrations and create the database

dotnet ef database update

Usage
Register as a user or login as admin.

Create support tickets.

Communicate via chat in tickets.

Receive real-time notifications on messages and ticket updates.

Project Structure
Domain: Entity classes and core business logic.
Infrastructure: Data access with EF Core.
Application: Services, interfaces, and DTOs.
Hubs: SignalR hub implementations.
UI: Razor Pages and client-side scripts.

Contributing
Contributions are welcome! Feel free to open issues or submit pull requests.

License
This project is licensed under the MIT License. See the LICENSE file for details.

Contact
Created by İlhan Yiğitoğlu
GitHub: https://github.com/Andunie
Email: ilhan_yigitoglu_@hotmail.com

Thank you for checking out this project!
