FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Abs.BooksCatalog.Service/Abs.BooksCatalog.Service.csproj", "Abs.BooksCatalog.Service/"]
COPY ["Abs.Messages/Abs.Messages.csproj", "Abs.Messages/"]
RUN dotnet restore "Abs.BooksCatalog.Service/Abs.BooksCatalog.Service.csproj"
COPY . .
WORKDIR "/src/Abs.BooksCatalog.Service"
RUN dotnet build "Abs.BooksCatalog.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Abs.BooksCatalog.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Abs.BooksCatalog.Service.dll"]