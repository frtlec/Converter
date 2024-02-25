FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /app
COPY Blazor.UI/*.csproj Blazor.UI/ 
RUN dotnet restore  Blazor.UI/*.csproj
COPY . ./
RUN dotnet publish Blazor.UI/*.csproj -c Release -o out
FROM nginx:latest
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY Blazor.UI/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/out/wwwroot /usr/share/nginx/html