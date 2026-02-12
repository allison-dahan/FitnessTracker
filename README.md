# Fitness Tracker

A comprehensive fitness tracking application built with ASP.NET Core 8.0, designed to help users track their workouts, nutrition, and fitness goals.

![Fitness Tracker Dashboard](https://placehold.co/800x400?text=Fitness+Tracker+Dashboard) 
*(Note: Replace with actual screenshot of your dashboard)*

## 🚀 Features

-   **Dashboard**: Visual overview of your fitness progress with interactive charts.
-   **Workout Logging**: Track various workout types (Running, Weightlifting, HIIT, etc.) with duration and calorie estimates.
-   **Nutrition Tracking**: Log meals (Breakfast, Lunch, Dinner, Snack) and monitor macronutrients (Protein, Carbs, Fats).
-   **Goal Setting**: Set and update personal fitness goals for steps, calories, and water intake.
-   **Responsive Design**: Mobile-friendly interface built with Bootstrap 5 and glassmorphism aesthetics.
-   **Secure Authentication**: Google Sign-In integration via ASP.NET Core Identity.

## 🛠️ Tech Stack

-   **Framework**: [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/apps/aspnet)
-   **Language**: [C# 10+](https://learn.microsoft.com/en-us/dotnet/csharp/)
-   **Database**: [MySQL](https://www.mysql.com/) (hosted on Azure Database for MySQL)
-   **ORM**: [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) with `Pomelo.EntityFrameworkCore.MySql`
-   **Frontend**: Razor Views, [Bootstrap 5](https://getbootstrap.com/), [Chart.js](https://www.chartjs.org/)
-   **Deployment**: Azure App Service with GitHub Actions CI/CD

## 🏁 Getting Started

### Prerequisites

-   [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-   [MySQL Server](https://dev.mysql.com/downloads/installer/) (Local or Docker)
-   Google Cloud Console Project (for OAuth)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/allison-dahan/FitnessTracker.git
    cd FitnessTracker
    ```

2.  **Configure Application Settings**
    Update `appsettings.json` (or use User Secrets) with your database connection string and Google Auth credentials:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=fitness;User=root;Password=your_password;"
    },
    "Authentication": {
      "Google": {
        "ClientId": "YOUR_CLIENT_ID",
        "ClientSecret": "YOUR_CLIENT_SECRET"
      }
    }
    ```

3.  **Run the Application**
    ```bash
    dotnet run --project FitnessTracker.Web
    ```
    The application will automatically create the database and seed initial data on the first run.

4.  **Access the App**
    Open your browser and navigate to `http://localhost:5000`.

## 📦 Deployment

This project is configured for continuous deployment to Azure App Service via GitHub Actions.

For details on setting up the deployment pipeline, see [DEPLOYMENT.md](DEPLOYMENT.md).

## 📄 License

This project is licensed under the MIT License.
