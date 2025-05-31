# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["AuthenticatorAppNew/AuthenticatorAppNew.csproj", "AuthenticatorAppNew/"]
RUN dotnet restore "AuthenticatorAppNew/AuthenticatorAppNew.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/AuthenticatorAppNew"
RUN dotnet publish "AuthenticatorAppNew.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Get the PORT from environment for Render compatibility
ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "AuthenticatorAppNew.dll"]
