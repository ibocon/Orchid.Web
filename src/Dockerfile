FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "Directory.Packages.props", "NuGet.config", ".editorconfig", "./"]
COPY ["src/Orchid.Web.csproj", "src/"]
RUN dotnet restore "src/Orchid.Web.csproj"

COPY ["src/", "src/"]
RUN dotnet publish "src/Orchid.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore -p:EnforceCodeStyleInBuild=false

FROM nginx:alpine AS final
COPY src/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
EXPOSE 8080
