#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ChoirManagement.Membership.WebService/ChoirManagement.Membership.WebService.csproj", "ChoirManagement.Membership.WebService/"]
RUN dotnet restore "ChoirManagement.Membership.WebService/ChoirManagement.Membership.WebService.csproj"
COPY . .
WORKDIR "/src/ChoirManagement.Membership.WebService"
RUN dotnet build "ChoirManagement.Membership.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChoirManagement.Membership.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoirManagement.Membership.WebService.dll"]