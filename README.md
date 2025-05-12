# SE-GA2: Inventory and Order Management Web Application

A simple inventory and order management web application built with ASP.NET MVC and Entity Framework.

## GitHub Repository
Clone the project from GitHub:  
[https://github.com/qchau0202/SE-GA2](https://github.com/qchau0202/SE-GA2)

## Prerequisites
- Visual Studio (latest)
- Microsoft SQL Server
- .NET Framework (compatible with the version in the solution)
- Entity Framework
- SQL Server Management Studio (SSMS) or any other SQL client

## Setting Up

### Clone the Repository
Use the following command to clone the repository:  
```bash
git clone https://github.com/qchau0202/SE-GA2.git
```

### Open the Project
Open the `.sln` file (e.g., `SE-GA2.sln`) in Visual Studio.

### Set Up the Database
1. Open SQL Server Management Studio.
2. Locate and open the `InventoryDB.sql` file included in the project.
3. Execute the entire script to perform the following:
   - Create the `InventoryDB` database.
   - Create required tables: `Item`, `Agent`, `Users`, `Order`, `OrderDetail`, `Cart`.
   - Insert sample data into tables.
   - Alter the `Order` table to include the `Status` and `UserID` columns.
4. Ensure that all `CREATE`, `INSERT`, and `ALTER` commands are executed in order.

### Update the Connection String
Update the connection string in the `Web.config` file with your SQL Server instance name. Example:  
```xml
<connectionStrings>
  <add name="InventoryDBEntities" connectionString="metadata=res://*/InventoryModel.csdl|res://*/InventoryModel.ssdl|res://*/InventoryModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=YOUR_SERVER_NAME;initial catalog=InventoryDB;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```
Replace `YOUR_SERVER_NAME` with your actual SQL Server instance (e.g., `CHOUCHOU` or `localhost`).

### Build and Run the Application
In Visual Studio, click “Start Without Debugging” or press `Ctrl + F5` to launch the web application.

## Login Information
**Cusomter credentials for login:**  
- Username: `john_doe`  
- Email: `john.doe@email.com`
- Password: `Pass123!`
Or just create a new account through the Register page

**Admin credentials for login:**  
- Username: `admin`  
- Email: `admin@gmail.com`
- Password: `admin123`

Additional users are already seeded in the SQL script.
