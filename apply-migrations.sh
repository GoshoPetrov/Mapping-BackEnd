#!/bin/bash

# Environment variables
DB_SERVER="db"
DB_NAME="${DB_NAME}"
SA_PASSWORD="${SA_PASSWORD}"

dotnet tool restore

sleep 5

export ConnectionStrings__DefaultConnection="Server=${DB_SERVER};Database=${DB_NAME};User Id=sa;Password=${SA_PASSWORD};Encrypt=True;TrustServerCertificate=True;"

# Drop the database -- it will be recreated
echo "Removing database '${DB_NAME}' ..."
/opt/mssql-tools/bin/sqlcmd -S "${DB_SERVER}" -U "sa" -P "${SA_PASSWORD}" -Q "DROP DATABASE ${DB_NAME}"

# Check if the database exists
echo "Checking if database '${DB_NAME}' exists..."
DB_EXISTS=$(/opt/mssql-tools/bin/sqlcmd -S "${DB_SERVER}" -U "sa" -P "${SA_PASSWORD}" -Q "SELECT COUNT(*) FROM sys.databases WHERE name = '${DB_NAME}'" -h -1)

if [ "${DB_EXISTS}" -eq 0 ]; then
    echo "Database '${DB_NAME}' does not exist. Creating it..."
    /opt/mssql-tools/bin/sqlcmd -S "${DB_SERVER}" -U "sa" -P "${SA_PASSWORD}" -Q "CREATE DATABASE ${DB_NAME}"
else
    echo "Database '${DB_NAME}' already exists."
fi

# Wait for the database to be ready
echo "Waiting for database to be ready..."
until /opt/mssql-tools/bin/sqlcmd -S "${DB_SERVER}" -U "sa" -P "${SA_PASSWORD}" -Q "SELECT 1" > /dev/null 2>&1; do
    echo "Database not ready, retrying in 5 seconds..."
    sleep 5
done

# Apply migrations
echo "Applying migrations..."
until dotnet ef database update --connection "Server=${DB_SERVER};Database=${DB_NAME};User Id=sa;Password=${SA_PASSWORD};Encrypt=True;TrustServerCertificate=True;"; do
    echo "Migrations failed, retrying in 5 seconds..."
    sleep 5
done

# Execute the init.sql script
echo "Executing init.sql script..."
/opt/mssql-tools/bin/sqlcmd -S "${DB_SERVER}" -U "sa" -P "${SA_PASSWORD}" -d "${DB_NAME}" -i /app/init.sql

# Migrations are applied, exit the container
echo "Migrations applied successfully."
