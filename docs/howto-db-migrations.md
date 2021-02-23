### Первичный запуск средства миграций в Entity Framework.
1. Установить терминальный инструмент работы с Entity Framework:
```
dotnet tool install --global dotnet-ef
```
3. Создать сущности в коде, обеспечить реализацию DbContext.
4. Создать первоначальные миграции:
```
dotnet ef migrations add Initial --startup-project=../library-test-task
```
5. Сгенерировать таблицы в базе данных: 
```
dotnet ef database update --context ApplicationContext --startup-project=../library-test-task
```

### Пересоздание базы данных.
Для пересоздания базы данных использовать команды:
```
dotnet ef database drop --force --context ApplicationContext --startup-project=../library-test-task
dotnet ef database update --context ApplicationContext --startup-project=../library-test-task
dotnet ef migrations add Initial --startup-project=../library-test-task
```