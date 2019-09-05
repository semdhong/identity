FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY ei8-Identity.sln ./
COPY main/Identity.csproj main/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/main
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "works.ei8.Identity.dll"]
