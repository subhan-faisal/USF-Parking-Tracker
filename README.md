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

## 🛠️ Technology Stack

### Frontend
- React.js with TypeScript for type safety
- Tailwind CSS for modern, responsive styling
- Shadcn UI components for consistent design
- React Hook Form with Zod validation
- Leaflet.js for interactive maps
- Recharts for data visualization
- TanStack Query (React Query) for efficient data fetching

### Backend
- Node.js with Express.js API framework
- PostgreSQL database with Drizzle ORM
- Session-based authentication with Passport.js
- REST API architecture
- Stripe payment integration

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

## 🌟 Team Members


- Jennifer Negron - >Home Page and CRUD Navigation
- Ronia Arabian - Responsible for backend architecture and Azure deployment
- Subhan Faisal - Responsive Layout & Styling, API and Azure deployment.
- Chandar Rathala -Responsible for database architecture, backend implementation, and OpenMap API integration and data processing.

## 📄 License

This project was created as part of a Dynamic Web Application Development course at the University of South Florida.
