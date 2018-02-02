FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app

# Expose port 80 for the Web API traffic
ENV ASPNETCORE_URLS "http://+:80, http://localhost:80"
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY *.sln ./
COPY SimpleWebApiPod/SimpleWebApiPod.csproj SimpleWebApiPod/
RUN dotnet restore
COPY . .
WORKDIR /src/SimpleWebApiPod
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SimpleWebApiPod.dll"]
