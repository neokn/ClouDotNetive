FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ClouDotNetive/ClouDotNetive.csproj", "ClouDotNetive/"]
RUN dotnet restore "ClouDotNetive/ClouDotNetive.csproj"
COPY . .
WORKDIR "/src/ClouDotNetive"
RUN dotnet build "ClouDotNetive.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClouDotNetive.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClouDotNetive.dll"]
