FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY API/*.csproj API/ 
RUN dotnet restore  API/*.csproj
COPY . .
RUN dotnet publish API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","API.dll" ]