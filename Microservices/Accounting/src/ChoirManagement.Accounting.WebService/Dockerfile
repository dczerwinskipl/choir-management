#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build

# NEvo Framework, TODO: create nuget packages
COPY ["Framework/src/NEvo.Core/NEvo.Core.csproj", "Framework/src/NEvo.Core/NEvo.Core.csproj"]
COPY ["Framework/src/NEvo.Azure/NEvo.Azure.csproj", "Framework/src/NEvo.Azure/NEvo.Azure.csproj"]
COPY ["Framework/src/NEvo.CQRS/NEvo.CQRS.csproj", "Framework/src/NEvo.CQRS/NEvo.CQRS.csproj"]
COPY ["Framework/src/NEvo.DomainDrivenDesign/NEvo.DomainDrivenDesign.csproj", "Framework/src/NEvo.DomainDrivenDesign/NEvo.DomainDrivenDesign.csproj"]
COPY ["Framework/src/NEvo.Sagas/NEvo.Sagas.csproj", "Framework/src/NEvo.Sagas/NEvo.Sagas.csproj"]
COPY ["Framework/src/NEvo.ValueObjects/NEvo.ValueObjects.csproj", "Framework/src/NEvo.ValueObjects/NEvo.ValueObjects.csproj"]

RUN dotnet restore "Framework/src/NEvo.Core/NEvo.Core.csproj"
RUN dotnet restore "Framework/src/NEvo.Azure/NEvo.Azure.csproj"
RUN dotnet restore "Framework/src/NEvo.CQRS/NEvo.CQRS.csproj"
RUN dotnet restore "Framework/src/NEvo.DomainDrivenDesign/NEvo.DomainDrivenDesign.csproj"
RUN dotnet restore "Framework/src/NEvo.Sagas/NEvo.Sagas.csproj"
RUN dotnet restore "Framework/src/NEvo.ValueObjects/NEvo.ValueObjects.csproj"

# Accounting Microservices
COPY ["Microservices/Accounting/src/ChoirManagement.Accounting.WebService/ChoirManagement.Accounting.WebService.csproj", "Microservices/Accounting/src/ChoirManagement.Accounting.WebService/"]
RUN dotnet restore "Microservices/Accounting/src/ChoirManagement.Accounting.WebService/ChoirManagement.Accounting.WebService.csproj"

COPY . .

WORKDIR "/build/Microservices/Accounting/src/ChoirManagement.Accounting.WebService"
RUN dotnet build "ChoirManagement.Accounting.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChoirManagement.Accounting.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoirManagement.Accounting.WebService.dll"]