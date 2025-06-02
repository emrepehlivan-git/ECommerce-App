FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/ECommerce.WebAPI/ECommerce.WebAPI.csproj", "src/Presentation/ECommerce.WebAPI/"]
COPY ["src/Core/ECommerce.Application/ECommerce.Application.csproj", "src/Core/ECommerce.Application/"]
COPY ["src/Core/ECommerce.Domain/ECommerce.Domain.csproj", "src/Core/ECommerce.Domain/"]
COPY ["src/Infrastructure/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "src/Infrastructure/ECommerce.Infrastructure/"]
COPY ["src/Infrastructure/ECommerce.Persistence/ECommerce.Persistence.csproj", "src/Infrastructure/ECommerce.Persistence/"]
RUN dotnet restore "src/Presentation/ECommerce.WebAPI/ECommerce.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/ECommerce.WebAPI"
RUN dotnet build "ECommerce.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommerce.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# CA sertifikasını sistem sertifika deposuna ekle
USER root
RUN apt-get update && apt-get install -y ca-certificates
COPY certs/ca.crt /usr/local/share/ca-certificates/ecommerce-ca.crt
RUN update-ca-certificates
USER $APP_UID

ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV ASPNETCORE_HTTPS_PORT=8081
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/webapi.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword

ENTRYPOINT ["dotnet", "ECommerce.WebAPI.dll"]
