# Multi-stage build for the FPT Booking backend API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore dependencies
COPY FPT_Booking_BE.csproj .
RUN dotnet restore FPT_Booking_BE.csproj

# Copy the remaining source and publish the app
COPY . .
RUN dotnet publish FPT_Booking_BE.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY entrypoint.sh ./
RUN chmod +x /app/entrypoint.sh

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["/app/entrypoint.sh"]
