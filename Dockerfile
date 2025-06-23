FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY SalesReports.sln ./
COPY SalesReports.Api/SalesReports.Api.csproj ./SalesReports.Api/
COPY SalesReports.App/SalesReports.App.csproj ./SalesReports.App/
COPY SalesReports.Domain/SalesReports.Domain.csproj ./SalesReports.Domain/
COPY SalesReports.Tests.Integration/SalesReports.Tests.Integration.csproj ./SalesReports.Tests.Integration/

RUN dotnet restore

COPY . ./
WORKDIR /app/SalesReports.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SalesReports.Api.dll"]
