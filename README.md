# orm-benchmark
A project to benchmark the read/write performance of Entity Framework Core vs Dapper with random data.

## Tools
- [Bogus](https://github.com/bchavez/Bogus)
- [BenchmarkDotNet](https://benchmarkdotnet.org/)

To run this benchmark on your local machine first **clone this repo locally**

Entity Framework Core tools
-----------------------------------------------------------------
If you don't have dotnet Entity Framework Core tools, install it

```bash
dotnet tool install --global dotnet-ef
```

Sql Server Database
--------------------
You need to have a local Sql Server Database or you can use docker-compose to run [docker-compose.yaml](https://github.com/thiagomotoca/orm-benchmark/blob/main/docker-compose.yaml)

```bash
docker-compose up
```

Run EF migrations
--------------------
Run migrations on you localdb by running the below command in you **package manager console**

```bash
dotnet ef database update --project OrmBenchmark.Data
```

Run benchmark
--------------------
Navigate to the folder where you have cloned this repo locally.
Nabigate to `\orm-benchmark\OrmBenchmark` and run the below command to start the benchmark and wait for it to complete to see the benchmark summary.

```bash
dotnet run -c Release
```