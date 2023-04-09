#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Microservices/Membership/src/ChoirManagement.MembersRegistration.WebService/ChoirManagement.MembersRegistration.WebService.csproj", "Microservices/Membership/src/ChoirManagement.MembersRegistration.WebService/"]
COPY ["Microservices/Shared/src/ChoirManagement.Bootstraping/ChoirManagement.Bootstraping.csproj", "Microservices/Shared/src/ChoirManagement.Bootstraping/"]
COPY ["Framework/src/NEvo.Azure/NEvo.Azure.csproj", "Framework/src/NEvo.Azure/"]
COPY ["Framework/src/NEvo.Core/NEvo.Core.csproj", "Framework/src/NEvo.Core/"]
COPY ["Framework/src/NEvo.CQRS/NEvo.CQRS.csproj", "Framework/src/NEvo.CQRS/"]
COPY ["Framework/src/NEvo.ValueObjects/NEvo.ValueObjects.csproj", "Framework/src/NEvo.ValueObjects/"]
RUN dotnet restore "Microservices/Membership/src/ChoirManagement.MembersRegistration.WebService/ChoirManagement.MembersRegistration.WebService.csproj"
COPY . .
WORKDIR "/src/Microservices/Membership/src/ChoirManagement.MembersRegistration.WebService"
RUN dotnet build "ChoirManagement.MembersRegistration.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChoirManagement.MembersRegistration.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoirManagement.MembersRegistration.WebService.dll"]