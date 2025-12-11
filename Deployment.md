# Deployment Azure Instructions: Week 16 - HospitalSupplyChainManagementSystem

This document describes how I would deploy the **HospitalSupplyChainManagementSystem** ASP.NET Core MVC project to Azure App Service using Visual Studio. I was unable to complete a live deployment because my Azure for Students subscription both basic and free subscription would not work (Screenshot included). I also changes the region and it still would not work for both basic and free plan.
If the sysatem would have allowed here is the breakdown step by step:

## 1. Create a Resource Group

1. Sign in to the Azure Portal at https://portal.azure.com.
2. In the search bar, type **Resource groups** and click **Create**.
3. Choose your subscription. (First tried free and then switch to basic)
4. Enter a name such as 'HSCMS-Group`.
5. Select a region close to you (for example, *East US*). (Tried both eastern and pacific since I am currently in california)
6. Click **Review + create → Create**.

## 2. Create an Azure App Service (Web App)

1. In the Azure Portal, search for **App Services**.
2. Click **Create → Web App**.
3. Select the `HSCMS-Group` resource group.
4. Set the **Name** to something like `hscms-tm` (this becomes `hscms-tm.azurewebsites.net`).
5. Set **Publish** to **Code**.
6. Set **Runtime stack** to **.NET 8 (LTS)**.
7. Set **Operating System** to **Windows**.
8. Choose a small pricing tier (Free, Shared, or Basic depending on what the subscription allows).
9. Click **Review + create → Create**.

## 3. Provision the SQL Database

The HospitalSupplyChainManagementSystem uses Entity Framework Core and SQL Server, so I would also provision an Azure SQL Database.

1. In the Azure Portal, search for **SQL databases** and click **Create**.
2. Use the same subscription and `HSCMS-Group` resource group.
3. Name the database `HSCMSDB`.
4. Create a new SQL server, for example `hscms-sql-tm`, and set an admin username and strong password.
5. Pick the lowest-cost compute tier available.
6. After deployment, open the SQL server → **Networking** and allow Azure services to access the server.

## 4. Configure the connection string and secrets

1. In the Portal, open the `HSCMSDB` database and go to **Connection strings**.
2. Copy the **ADO.NET** connection string.
3. Open the App Service (`hscms-tm`) → **Configuration → Connection strings**.
4. Add a new connection string named **`DefaultConnection`** (this matches `builder.Configuration.GetConnectionString("DefaultConnection")` in `Program.cs`).
5. Paste the ADO.NET connection string value and set the type to **SQLAzure**.
6. Save the configuration and let the App Service restart.
7. In the project, `appsettings.Production.json` should contain a `ConnectionStrings:DefaultConnection` key, but with either an empty string or a placeholder so that no secrets are committed.

## 5. Publish from Visual Studio

1. Open `HospitalSupplyChainManagementSystem.sln` in Visual Studio.
2. Right-click the **HospitalSupplyChainManagementSystem** project → **Publish…**.
3. Choose **Azure → Azure App Service (Windows) → Next**.
4. Sign in with the same Azure account. (It found my accound)
5. Select the `hscms-tm` App Service and click **Finish**.
6. In the Publish profile, set **Configuration** to **Release**.
7. Click **Publish**. Visual Studio builds the project and deploys it to Azure.
8. Once the browser opens, I would verify the home page and the `/healthz` endpoint at `https://hscms-tm.azurewebsites.net/healthz`.

Although I cannot complete these steps on my current Azure subscription, this process describes a full, production-style deployment of the HospitalSupplyChainManagementSystem using Visual Studio and Azure App Service, including resource group selection, runtime selection, database provisioning, environment variable configuration, and the final publish step.
