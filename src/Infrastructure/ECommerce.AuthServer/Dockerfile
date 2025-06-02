FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Infrastructure/ECommerce.AuthServer/ECommerce.AuthServer.csproj", "src/Infrastructure/ECommerce.AuthServer/"]
COPY ["src/Infrastructure/ECommerce.Persistence/ECommerce.Persistence.csproj", "src/Infrastructure/ECommerce.Persistence/"]
COPY ["src/Core/ECommerce.Application/ECommerce.Application.csproj", "src/Core/ECommerce.Application/"]
COPY ["src/Core/ECommerce.Domain/ECommerce.Domain.csproj", "src/Core/ECommerce.Domain/"]
COPY ["src/Infrastructure/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "src/Infrastructure/ECommerce.Infrastructure/"]
RUN dotnet restore "src/Infrastructure/ECommerce.AuthServer/ECommerce.AuthServer.csproj"
COPY . .
WORKDIR "/src/src/Infrastructure/ECommerce.AuthServer"
RUN dotnet build "ECommerce.AuthServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommerce.AuthServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# CA sertifikasını sistem sertifika deposuna ekle
RUN apt-get update && apt-get install -y ca-certificates
COPY certs/ca.crt /usr/local/share/ca-certificates/ecommerce-ca.crt
RUN update-ca-certificates

ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV ASPNETCORE_HTTPS_PORT=8081
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/auth.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword

ENTRYPOINT ["dotnet", "ECommerce.AuthServer.dll"]
