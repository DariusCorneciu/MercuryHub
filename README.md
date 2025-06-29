# Mercury HUB

**Mercury HUB** is a WPF desktop application designed to help front desk staff manage and assign bookings received from the [Mercury Booking](https://github.com/DariusCorneciu/MercuryApp) website. It communicates with a REST API to synchronize data and provides a clean interface for handling daily tasks in hospitality management.

## 🔧 Features

- ✅ API key-based authentication (unique per user)
- 📅 View and filter unassigned bookings
- 🔍 Search and match bookings to existing guests
- 🏨 Assign rooms manually for each reservation
- 💾 Local saving and sync with the central database
- 📤 Integration with Mercury Booking system

## 🏗️ Tech Stack

- WPF (.NET 6/7)
- ASP.NET Core REST API
- Entity Framework Core
- SQLite (for local storage, if used)
- Swagger for API testing

## 🧪 Requirements

- .NET Desktop Runtime 6.0 or newer
- Active internet connection to access the API
- Valid API key

## 🔑 Authentication

On first launch, the app will ask for an API key. This key is stored locally, so the user does not need to enter it again later.

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/username/mercury-hub.git
   cd mercury-hub
