Setting Up
=========

Clone the Repository
-------------------
Clone the repository using:
git clone https://github.com/qchau0202/SE-GA2.git

Open the Project
----------------
Open the SE-GA2.sln file in Visual Studio.

Set Up the Database
-------------------
1. Open SQL Server Management Studio.
2. Find and run the InventoryDB.sql file from the project.
3. The script will:
   - Create the InventoryDB database.
   - Create tables: Item, Agent, Users, Order, OrderDetail, Cart.
   - Add sample data.
   - Add Status and UserID columns to the Order table.
4. Run all commands in order.

Update the Connection String
---------------------------
Update the connection string in Web.config with your SQL Server instance name. Example:
<connectionStrings>
  <add name="InventoryDBEntities" connectionString="metadata=res://*/InventoryModel.csdl|res://*/InventoryModel.ssdl|res://*/InventoryModel.msl;provider=System.Data.SqlClient;provider connection string='data source=YOUR_SERVER_NAME;initial catalog=InventoryDB;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework'" providerName="System.Data.EntityClient" />
</connectionStrings>
Replace YOUR_SERVER_NAME with your SQL Server instance (e.g., CHOUCHOU or localhost).

Build and Run
-------------
In Visual Studio, press Ctrl + F5 to start the application.