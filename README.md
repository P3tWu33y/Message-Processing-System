# Message-Processing-System
A Message Processing System


## Prerequisites

Before running the services, make sure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis Server](https://redis.io/download)

# Create the database:
Use the file Create_Schema.sql and execute the commands in your MSSQL so it will create the database.

--SQL username and password must fit the code in which the username and password is:
"Server=localhost; Database=main; User Id=sa; Password=220497;"



# Run your Redis Server:
redis-server --daemonize yes

## Install dependencies for each service and build:

# ServiceA
cd ServiceA
dotnet restore && dotnet build

# ServiceB
cd ../ServiceB
dotnet restore && dotnet build

# ServiceC
cd ../ServiceC
dotnet restore && dotnet build

-Now you can run Service B and Service C and when they're loaded and runnning you may run ServiceA to generate data --
(if needed run it multiple times just to create more data).



