# Products API — Clean Architecture + CQRS (.NET 10)


```bash
dotnet restore       
dotnet run --project src/ProductsApi.Api
```
## EndPoints

| Method | Route | description |
|---|---|---|
| GET | /api/products | all products |
| GET | /api/products/{id} | one product (404 if not found) |
| POST | /api/products | `{ "name": "Laptop", "price": 15000 }` |

