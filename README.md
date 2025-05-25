# KPO_BDZ_2: Микросервисная система хранения и анализа файлов

## Описание
Проект состоит из трёх микросервисов:
- **FileStoringService** — сервис хранения файлов и метаданных
- **FileAnalysisService** — сервис анализа и сравнения файлов
- **ApiGateway** — API-шлюз для маршрутизации запросов

Используется база данных PostgreSQL.

## Запуск через Docker Compose
1. Убедитесь, что установлен Docker и Docker Compose.
2. В корне проекта выполните:
   ```
   docker-compose up --build
   ```
3. Сервисы будут доступны по портам:
   - ApiGateway: http://localhost:5000
   - FileStoringService: http://localhost:5001
   - FileAnalysisService: http://localhost:5002
   - PostgreSQL: localhost:5432 (user: postgres, password: postgres)

## Основные эндпоинты
### FileStoringService
- `POST /api/files/upload` — загрузка файла
- `GET /api/files/{id}` — скачивание файла

### FileAnalysisService
- `POST /api/analysis/analyze/{fileId}` — анализ файла
- `GET /api/analysis/compare/{fileId1}/{fileId2}` — сравнение файлов
- `GET /api/analysis/wordcloud/{fileId}` — генерация облака слов для файла (возвращает JSON с частотами слов)

### ApiGateway
- `POST /api/gateway/upload` — загрузка файла через шлюз
- `GET /api/gateway/files/{id}` — скачивание файла через шлюз
- `POST /api/gateway/analyze/{fileId}` — анализ файла через шлюз
- `GET /api/gateway/compare/{fileId1}/{fileId2}` — сравнение файлов через шлюз
- `GET /api/gateway/wordcloud/{fileId}` — генерация облака слов через шлюз (проксирует запрос к FileAnalysisService)

## Миграции и инициализация БД
Миграции выполняются автоматически при запуске сервисов. Для ручного применения миграций используйте:
```
dotnet ef database update
```
в директории соответствующего сервиса.

## Тестирование
Тесты рекомендуется размещать в отдельных проектах с использованием xUnit или аналогичного фреймворка. Запуск тестов:
```
dotnet test
```

## Пример запроса на анализ файла
```
curl -X POST http://localhost:5002/api/analysis/analyze/{fileId}
```

## Пример запроса на генерацию облака слов
```
curl http://localhost:5002/api/analysis/wordcloud/{fileId}
```
или через шлюз:
```
curl http://localhost:5000/api/gateway/wordcloud/{fileId}
```

## Примечания
- Все сервисы используют переменные окружения для конфигурации подключения к БД.
- Для разработки рекомендуется использовать режим Development.
- Для запуска fileanalysisservice используйте порт 6002.
- Пример запроса на сравнение файлов:

```
curl -s http://localhost:6002/api/analysis/compare/ID1/ID2
```

- Пример запроса на анализ файла:

```
curl -X POST http://localhost:6002/api/analysis/analyze/ID1
```