# Fitness Tracker - Azure Deployment Guide

This guide walks you through deploying the Fitness Tracker application to Azure App Service with a MySQL database.

## Prerequisites

- **Azure Account**: You need an active Azure subscription. [Create one for free](https://azure.microsoft.com/free/).
- **GitHub Account**: Your code should be pushed to a GitHub repository (which it is!).

---

## Step 1: Create Azure Resources

1.  Log in to the [Azure Portal](https://portal.azure.com).
2.  Click **Create a resource**.
3.  Search for **Web App + Database** and select it.
4.  Click **Create**.
5.  Fill in the details:
    -   **Resource Group**: Create a new one (e.g., `rg-fitness-tracker`).
    -   **Region**: Select a region close to you (e.g., `East US`).
    -   **Name**: Choose a unique name (e.g., `fitness-tracker-allison`). This will be your URL (`https://fitness-tracker-allison.azurewebsites.net`).
    -   **Runtime Stack**: `.NET 8 (LTS)`.
    -   **Engine**: `MySQL - Flexible Server`.
    -   **Hosting Plan**: Select `Basic` or `Standard` for production features, or `Free` (if available, but usually doesn't support MySQL integration as easily in this wizard). *Recommendation: Start with Basic B1 for testing.*
6.  Click **Review + create**, then **Create**.
    -   *Note: This process may take 5-10 minutes.*

---

## Step 2: Configure Application Settings

Once the deployment is complete, go to your new **App Service** resource.

1.  In the left menu, under **Settings**, click on **Environment variables**.
2.  Click **Add** to add the following settings:

    | Name | Value |
    | :--- | :--- |
    | `ASPNETCORE_ENVIRONMENT` | `Production` |
    | `Authentication__Google__ClientId` | *Your Google Client ID* |
    | `Authentication__Google__ClientSecret` | *Your Google Client Secret* |

    *Note: The Connection String for the database should have been automatically added by Azure during creation. You can verify this under the "Connection strings" tab in Environment variables. It is usually named `defaultConnection` or `azuremysql`.*

3.  Click **Apply** and then **Confirm**.

---

## Step 3: Setup Continuous Deployment

1.  In the App Service left menu, under **Deployment**, click **Deployment Center**.
2.  **Source**: Select **GitHub**.
3.  **Authorize** Azure to access your GitHub account if needed.
4.  **Organization**: Select your GitHub username.
5.  **Repository**: Select `FitnessTracker`.
6.  **Branch**: Select `main`.
7.  Click **Save**.

Azure will now set up a GitHub Actions workflow to build and deploy your app. You can monitor the progress in the "Actions" tab of your GitHub repository.

---

## Step 4: Configure Google Authentication

Your app needs to know its new production URL, and Google needs to trust it.

1.  Go to the [Google Cloud Console](https://console.cloud.google.com/).
2.  Navigate to **APIs & Services** > **Credentials**.
3.  Edit your existing OAuth 2.0 Client ID.
4.  Under **Authorized redirect URIs**, click **Add URI**.
5.  Enter your Azure URL with the callback path:
    ```
    https://<your-app-name>.azurewebsites.net/signin-google
    ```
    *(Replace `<your-app-name>` with the name you chose in Step 1).*
6.  Click **Save**.

---

## Step 5: Verify Deployment

1.  Wait for the GitHub Action to complete (check the Actions tab in your repo).
2.  Navigate to your app's URL (`https://<your-app-name>.azurewebsites.net`).
3.  The first load might take a minute as it starts up and the database is initialized.
4.  **Important**: The application will automatically create the database structure and seed default data (workouts, meals) the first time it runs.
5.  Try logging in with Google.
6.  Test the Dashboard and features!

---

## Troubleshooting

-   **App won't start?** Check **Log Stream** in the Azure App Service menu to see application errors.
-   **Database Error?** Ensure the connection string "Name" in Azure matches the code (it usually defaults to `defaultConnection` which our code supports). If Azure named it something else, you might need to update the Environment Variable name in Azure to `ConnectionStrings__DefaultConnection`.
