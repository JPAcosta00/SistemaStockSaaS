# 1. Etapa de compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución y todos los .csproj manteniendo la estructura
COPY API/*.csproj ./API/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infraestructure/*.csproj ./Infraestructure/

# Restaurar dependencias desde el proyecto principal API
RUN dotnet restore API/API.csproj

# Copiar todo el código fuente
COPY . .

# Compilar y publicar
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/out

# 2. Etapa de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]