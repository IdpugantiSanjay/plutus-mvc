
##### script to build docker image from root folder
docker build -t plutus-mvc -f Plutus.Mvc/Dockerfile .

##### script to run docker container
docker run -d --name plutus-mvc -p 9090:80 -e NpgSqlConnectionString="Host=192.168.29.23;Username=postgres;Password=postgres;Database=plutus"  plutus-mvc

#### pre-commit hook
```bash
#!/bin/sh
LC_ALL=C
# Select files to format
FILES=$(git diff --cached --name-only --diff-filter=ACM "*.cs" | sed 's| |\\ |g')
[ -z "$FILES" ] && exit 0

# Format all selected files
echo "$FILES" | cat | xargs | sed -e 's/ /,/g' | xargs dotnet format --include

# Add back the modified files to staging
echo "$FILES" | xargs git add

exit 0
```
