FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["GrpcService/GrpcService.csproj", "GrpcService/"]
RUN dotnet restore "GrpcService/GrpcService.csproj"
COPY . .
WORKDIR "/src/GrpcService"
RUN dotnet build "GrpcService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcService.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p dev-cert-password

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /https /https
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_HTTPS_PORTS=8443
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=dev-cert-password
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENTRYPOINT ["dotnet", "GrpcService.dll"]
