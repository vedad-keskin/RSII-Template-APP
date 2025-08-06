# CallTaxi - Complete Taxi Booking System

A comprehensive taxi booking system with .NET Web API backend and Flutter mobile applications.

## 🏗️ Project Structure

```
CallTaxi/
├── CallTaxi.WebAPI/          # Main API project
├── CallTaxi.Services/        # Business logic and data access
├── CallTaxi.Model/           # Data models and DTOs
├── CallTaxi.Subscriber/      # Background notification service
├── UI/
│   ├── calltaxi_desktop_admin/    # Admin dashboard (Flutter)
│   ├── calltaxi_mobile_client/    # Customer mobile app (Flutter)
│   └── calltaxi_mobile_driver/    # Driver mobile app (Flutter)
├── docker-compose.yml
├── Dockerfile
└── Dockerfile.notifications
```

### Desktop Admin Dashboard
- **Location**: `UI/calltaxi_desktop_admin/`
- **Purpose**: Administrative interface for managing vehicles, drivers, and business operations
- **Features**: Brand management, vehicle monitoring, business reports

### Admin
- **Username:** admin
- **Password:** test

### Mobile Driver App
- **Location**: `UI/calltaxi_mobile_driver/`
- **Purpose**: Driver-facing mobile application
- **Features**: Ride acceptance, navigation, earnings tracking

### Driver
- **Username:** driver
- **Password:** test

### Mobile Client App
- **Location**: `UI/calltaxi_mobile_client/`
- **Purpose**: Customer-facing mobile application
- **Features**: Ride booking, payment integration, real-time tracking

### Regular User-Client
- **Username:** user
- **Password:** test

### Web API
- **Port**: 5130
- **Features**: RESTful API endpoints, authentication, business logic
- **Swagger**: Available at `http://localhost:5130/swagger`

### Subscriber Service
- **Port**: 7111
- **Features**: Background processing, email notifications, RabbitMQ integration

### Database
- **SQL Server**: Port 1401
- **Database**: CallTaxiDb
- **Features**: Entity Framework Core, migrations, seeding

### Message Queue
- **RabbitMQ**: Ports 5672 (AMQP), 15672 (Management)
- **Features**: Asynchronous messaging, notification processing

## RabbitMQ Test E-mail Adresses

### An email is sent after a vehicle is created or modified by a driver to the system administrator to notify them that the registration needs to be approved or rejected.

### Sender E-mail Adress
- **Email:** calltaxi.sender@gmail.com
- **Password:** calltaxitest

### Receiver E-mail Adress
- **Email:** calltaxi.receiver@gmail.com
- **Password:** calltaxitest

## Recommender System

### Recommender System uses Content-Based Filtering, it suggests vehicle tier for the drive you are about to request based on a previous drives of the user and also time of the day, and day of the week.

## Stripe Payment System

### Stipe Test Card
- **Card Number:** 4242 4242 4242 4242
- **Rest of the data:** Can be done arbitrarily

## 4 .env Files
- **Root the of project:** For sql and rabbitmq configuration
- **Root of the desktop-admin app:** For OpenRouteService api key 
- **Root of the mobile-driver app:** For OpenRouteService api key 
- **Root of the mobile-client app:** For OpenRouteService api key and Stripe keys

## 🛠️ Development Commands

# Create new migration
dotnet ef migrations add MigrationName --project .\CallTaxi.Services --startup-project .\CallTaxi.WebAPI

# Executes the migrations
update-database

# Build and start all services
docker-compose up --build

# Navigate to Flutter project
cd UI/calltaxi_desktop_admin

# Get dependencies
flutter pub get

# Generate code (after adding new models)
dart run build_runner build

# Desktop Admin
cd UI/calltaxi_desktop_admin
flutter run -d windows  # or -d macos, -d linux

# Mobile Client
cd UI/calltaxi_mobile_client
flutter run -d chrome   # for web
flutter run -d ime-device-a  # for Android device

# Mobile Driver
cd UI/calltaxi_mobile_driver
flutter run -d chrome   # for web
flutter run -d ime-device-a  # for Android device