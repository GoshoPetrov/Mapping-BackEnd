# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ./backend/Mapping-BackEnd

# Copy the project file and restore any dependencies
COPY ./backend/Mapping-BackEnd/*.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY ./backend/Mapping-BackEnd ./

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the port your application will run on
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Mapping-BackEnd.dll"]