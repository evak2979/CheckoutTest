#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . /
RUN dotnet restore "./Checkout.Web/Checkout.Web.csproj"
RUN dotnet build "./Checkout.Web/Checkout.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Checkout.Web/Checkout.Web.csproj" -c Release -o /app/publish
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https

ENTRYPOINT ["dotnet", "/app/publish/Checkout.Web.dll"]

