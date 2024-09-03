# Задание
[ТЗ](docs/task.md)

# Локальный запуск
Поднимаем БД в докере:
```powershell
cd deps
docker-compose up -d
```
Используя [`dotnet ef`](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) применяем миграции к БД:
```powershell
dotnet ef database update --project ./src/Hospital.DataAccess/Hospital.DataAccess.csproj --startup-project ./src/Hospital.Application/Hospital.Application.csproj --context Hospital.DataAccess.HospitalDbContext --configuration Release
```

Запускаем проект `Hospital.Application`. По умолчанию используются порты 4999, 5000

Запросы можно отправлять через [Swagger](https://localhost:5000/swagger).

# Тесты
Тестами для примера покрыт только `DoctorService`

```bash
dotnet test
```