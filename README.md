# EcommerceNetAngular

ECommerce with clean architecture based on unit of work with repository pattern in .NET 6.

## Getting Started

#### Running docker containers

from repository root folder

```shell
docker compose -f docker-compose.Development.yml up -d
```

#### Installing Angular dependencies

from Angular client (.../Client)

```shell
npm install
```

#### Running for local development

from Angular client (.../Client)

```shell
npm serve
```

from API (.../Fnunez.Ena.API)

```shell
dotnet run --launch-profile=https
```

#### Deploy Angular client into API wwwroot folder

from Angular client (.../Client)

```shell
npm build
```