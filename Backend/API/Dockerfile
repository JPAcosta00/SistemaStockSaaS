# 1. Etapa de compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y compilar
COPY . ./
RUN dotnet publish -c Release -o /app/out

# 2. Etapa de ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .

# Render asigna el puerto automáticamente mediante la variable PORT
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "NombreDeTuProyecto.dll"]