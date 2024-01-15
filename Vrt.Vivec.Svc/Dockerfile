#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Vrt.Vivec.Svc/Vrt.Vivec.Svc.csproj", "Vrt.Vivec.Svc/"]
RUN dotnet restore "Vrt.Vivec.Svc/Vrt.Vivec.Svc.csproj"
COPY . .
WORKDIR "/src/Vrt.Vivec.Svc"
RUN dotnet build "Vrt.Vivec.Svc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vrt.Vivec.Svc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vrt.Vivec.Svc.dll"]