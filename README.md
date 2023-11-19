# Message-Processing-System
A Message Processing System


## Prerequisites

Before running the services, make sure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis Server](https://redis.io/download)

# Create the database:
Use the file Create_Schema.sql and execute the commands in your MSSQL so it will create the database.

# Run your Redis Server:
redis-server --daemonize yes

## Install dependencies for each service:

# ServiceA
cd ServiceA
dotnet restore

# ServiceB
cd ../ServiceB
dotnet restore

# ServiceC
cd ../ServiceC
dotnet restore

-Now you can run Service B and Service C and when they're loaded and runnning you may run ServiceA to generate data for the first column 
(if needed run it multiple times just to create more data).
