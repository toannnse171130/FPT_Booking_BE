#!/bin/sh
set -e

# Bind Kestrel to the port provided by the hosting platform (defaults to 8080 locally)
export ASPNETCORE_URLS="http://+:${PORT:-8080}"

exec dotnet FPT_Booking_BE.dll
