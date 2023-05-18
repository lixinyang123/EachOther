## EachOther

Privacy Space

#### Install pre-requisites

- Visual Studio 2022 / Visual Studio Code
- .NET 7.0 SDK
- Libman (VS Extension)
- Bundler & Minifier (VS Extension)
- Mysql
- Docker（WSL2 is recommended）

> If you use Visual Studio (Not Visual Studio Code), you don't need to install Libman CLI and Entity Framework CLI.

Install Libman CLI：

```shell
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

Install Entity Framework Core CLI

```shell
dotnet tool install -g dotnet-ef
```

Configuration 

- src
    - appsettings.json

#### Configure database:

```
  "EachOther": "server=[host];user id=[user];password=[password];database=eachother"
```

#### Migrate Database

- Visual Studio

Tools \-\> Nuget Package Manager \-\> Package Manager Console

```shell
Update-Database
```

- Visual Studio Code or CLI

```shell
dotnet-ef database update -p ..\CoreHome.Data
```

#### Startup

- Visual Studio or Visual Studio Code

Click `Startup` in Solution Explorer or `Ctrl+F5`.

- CLI 

```shell
dotnet run
```

#### Build Dockerfile

- Visual Studio

Click `Build Dockerfile` in Solution Explorer.

- Visual Studio Code or CLI

Execute the following command in the project root directory.

```shell
docker build --tag eachother:latest .
```