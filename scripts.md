docker build -t plutus-mvc -f Plutus.Mvc/Dockerfile .

docker run -d --name plutus-mvc -p 9090:80 -e NpgSqlConnectionString="Host=192.168.29.23;Username=postgres;Password=postgres;Database=plutus"  plutus-mvc
