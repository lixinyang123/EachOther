FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ./src .
COPY ./CKEditor_Config.js ./CKEditor_Config.js
RUN dotnet restore

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli

RUN ~/.dotnet/tools/libman restore
RUN mv ./CKEditor_Config.js ./wwwroot/lib/ckeditor4/config.js
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EachOther.dll","urls=http://*"]
