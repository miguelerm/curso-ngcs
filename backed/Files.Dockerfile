FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Abs.FilesManager.Services/Abs.FilesManager.Services.csproj", "Abs.FilesManager.Services/"]
COPY ["Abs.Messages/Abs.Messages.csproj", "Abs.Messages/"]
RUN dotnet restore "Abs.FilesManager.Services/Abs.FilesManager.Services.csproj"
COPY . .
WORKDIR "/src/Abs.FilesManager.Services"
RUN dotnet build "Abs.FilesManager.Services.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Abs.FilesManager.Services.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Abs.FilesManager.Services.dll"]