# Utilizeaza o imagine de baza pentru SDK-ul .NET Core
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Seteaza directorul de lucru în container
WORKDIR /app

# Copiaza fi?ierele de proiect .csproj ?i restaureaza dependen?ele
COPY ["ITPLibrary.Api.sln", "."]
COPY ["ITPLibrary.Api/ITPLibrary.Api.csproj", "ITPLibrary.Api/"]
COPY ["ITPLibrary.Api.Core/ITPLibrary.Api.Core.csproj", "ITPLibrary.Api.Core/"]
COPY ["ITPLibrary.Api.Data/ITPLibrary.Api.Data.csproj", "ITPLibrary.Api.Data/"]
RUN dotnet restore "ITPLibrary.Api.sln"

# Copiaza restul codului sursa
COPY . .

# Publica aplica?ia pentru a fi rulata
WORKDIR /app/ITPLibrary.Api
RUN dotnet publish -c Release -o out

# Creeaza o imagine finala, mai mica, pentru rulare
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/ITPLibrary.Api/out ./
ENTRYPOINT ["dotnet", "ITPLibrary.Api.dll"]