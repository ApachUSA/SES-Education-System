#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SES.Web/SES.Web.csproj", "SES.Web/"]
COPY ["SES.Infrastructure/SES.Infrastructure.csproj", "SES.Infrastructure/"]
COPY ["SES.Domain/SES.Domain.csproj", "SES.Domain/"]
COPY ["SES.Service/SES.Service.csproj", "SES.Service/"]
RUN dotnet restore "SES.Web/SES.Web.csproj"
COPY . .
WORKDIR "/src/SES.Web"
RUN dotnet build "SES.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SES.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SES.Web.dll"]