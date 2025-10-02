# =========================
# Etapa 1: Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto para restaurar dependencias primero (cache)
COPY BicTechBack.sln ./
COPY src/Application/*.csproj ./src/Application/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Presentation/*.csproj ./src/Presentation/

# Restaurar dependencias
RUN dotnet restore BicTechBack.sln

# Copiar el resto del código
COPY . .

# Build y publish
RUN dotnet publish BicTechBack.sln -c Release -o /app

# =========================
# Etapa 2: Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar archivos publicados desde la etapa build
COPY --from=build /app .

# Comando de inicio
ENTRYPOINT ["dotnet", "Presentation.dll"]


