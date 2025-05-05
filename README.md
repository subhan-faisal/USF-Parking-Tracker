# USF Parking Tracker

A comprehensive parking management web application providing intelligent, user-centric parking solutions for urban commuters and university campus parking, with advanced data visualization and reservation capabilities.

## 📋 Features

- **Interactive Map**: View parking spots on an interactive map with real-time availability
- **Parking Reservation System**: Reserve parking spots in advance with flexible duration options
- **Secure Payment Processing**: Complete payment securely via Stripe or USF Wallet integration
- **User Authentication**: Secure login/registration system with personalized experiences
- **Favorites Management**: Save and quickly access your preferred parking locations
- **Data Visualization**: View parking trends, usage patterns, and availability statistics
- **Responsive Design**: Optimized for mobile, tablet, and desktop devices
- **University of Tampa Integration**: Special highlighting and features for University of Tampa parking locations

## 🚀 Tech Stack

### 🔷 Frontend

- **HTML/CSS**
- **JavaScript**
- **Bootstrap** – for responsive UI components  
- **jQuery** – with **jQuery Validation** for client-side form validation  
- **Leaflet.js** – for interactive map rendering

### 🛠️ Backend

- **ASP.NET Core 9.0** –
- **C#** – primary backend language
- **Entity Framework Core 9.0** – for database access and ORM
- **Microsoft SQL Server** – configured via `Microsoft.Data.SqlClient`

### 🧱 Architecture & Patterns

- **MVC (Model-View-Controller)** – layered application structure
- **Repository Pattern** – decouples business logic from data access
- **Entity Framework Code First** – model-driven database schema (see `Migrations/`)
- **RESTful API** – built under `Api/ParkingLotsController.cs`

### 💻 Development Tools

- **Visual Studio** or **Visual Studio Code**
- **EF Core CLI Tools** – for managing migrations and database schema
- **Git** – for version control and collaboration

### Alternative Implementation
- ASP.NET Core with C# (conversion available in the codebase)
- Entity Framework Core for data access
- Repository pattern and Unit of Work pattern
- Clean architecture principles

## 📊 Data Model

The application uses a relational database with the following key entities:

- **Users**: Authentication and user information
- **Parking Spots**: Location, availability, pricing information
- **Reservations**: Booking details linked to users and parking spots
- **Payments**: Transaction records with payment status
- **Favorites**: User's saved parking locations

## 🚀 Deployment

The application is designed to be deployed on Azure cloud platform and includes both JavaScript/TypeScript and C# implementations to accommodate different deployment scenarios.

### Prerequisites

- Node.js 18+ or .NET 7+
- PostgreSQL database
- Stripe account for payment processing

### Environment Variables

```
DATABASE_URL=postgresql://user:password@localhost:5432/dbname
STRIPE_SECRET_KEY=your_stripe_secret_key
VITE_STRIPE_PUBLIC_KEY=your_stripe_public_key 
```

## 💻 Development

### Running the JavaScript/TypeScript Version

```bash
# Install dependencies
npm install

# Set up environment variables
cp .env.example .env
# Edit .env with your values

# Run database migrations
npm run db:push

# Start development server
npm run dev
```

### Running the C# Version

```bash
# Navigate to the C# solution directory
cd USFParkingApp

# Restore packages
dotnet restore

# Set up database connection string in appsettings.json

# Run migrations
dotnet ef database update

# Run the application
dotnet run --project USFParkingApp.Web
```

## 📱 Mobile Responsiveness

The application features a fully responsive design that adapts to different screen sizes and devices:

- **Mobile**: Optimized for on-the-go usage with streamlined interface
- **Tablet**: Enhanced layout with more visible options
- **Desktop**: Full-featured experience with expanded visualizations

## 🔒 Security Features

- Password hashing using scrypt
- Session-based authentication
- CSRF protection
- Input validation with Zod schemas
- Secure payment processing with Stripe


## 🌟 Team Members & Reflections

### Jennifer Negron
- **Role**: Home Page and CRUD Navigation
- **Time Spent**: ~40 hours
- **Key Learnings**: Mastered React components, learned state management
- **Challenges**: Implementing real-time updates for parking availability
- **Improvements**: Suggest implementing WebSocket for real-time updates

### Ronia Arabian
- **Role**: Backend Architecture and Deployment
- **Time Spent**: ~45 hours
- **Key Learnings**: Gained expertise in ASP.NET Core and Entity Framework
- **Challenges**: Setting up proper database relationships and migrations
- **Improvements**: Implement caching for better performance

### Subhan Faisal
- **Role**: Responsive Layout & Styling, API Integration
- **Time Spent**: ~35 hours
- **Key Learnings**: Advanced Tailwind CSS, responsive design patterns
- **Challenges**: Cross-browser compatibility issues
- **Improvements**: Add more automated testing for UI components

### Chandar Rathala
- **Role**: Database Architecture and OpenMap Integration
- **Time Spent**: ~42 hours
- **Key Learnings**: Spatial data handling, API integration
- **Challenges**: Optimizing database queries for map data
- **Improvements**: Implement clustering for map markers
## 📄 License

This project was created as part of a Dynamic Web Application Development course at the University of South Florida.
