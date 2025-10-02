# =========================
# Etapa 1: Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el csproj del proyecto Presentation
COPY src/Presentation/Presentation.csproj ./Presentation/
WORKDIR /src/Presentation
RUN dotnet restore

# Copiar todo el código
WORKDIR /src
COPY . .

# Publicar el proyecto Presentation
WORKDIR /src/Presentation
RUN dotnet publish -c Release -o /app

# =========================
# Etapa 2: Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa anterior
COPY --from=build /app .

# Comando de inicio
ENTRYPOINT ["dotnet", "Presentation.dll"]
