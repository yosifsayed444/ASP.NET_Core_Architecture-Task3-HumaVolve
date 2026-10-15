# Products API — Clean Architecture + CQRS (.NET 10)

## التشغيل

المتطلبات: .NET 10 SDK + SQL Server (أو LocalDB اللي جاي مع Visual Studio).

```bash
dotnet restore        # بينزّل أحدث packages (الـ versions في الـ csproj مكتوبة 10.* / 12.* / 2.*)
dotnet run --project src/ProductsApi.Api
```

- لو SQL Server غير LocalDB: غيّر `ConnectionStrings:DefaultConnection` في `src/ProductsApi.Api/appsettings.json`.
- في وضع Development الـ Database والجدول بيتعملوا تلقائيًا (`EnsureCreated`).
- افتح `https://localhost:<port>/scalar/v1` لتجربة الـ endpoints.

## الـ Endpoints

| Method | Route | الوصف |
|---|---|---|
| GET | /api/products | كل المنتجات |
| GET | /api/products/{id} | منتج واحد (404 لو مش موجود) |
| POST | /api/products | إنشاء منتج `{ "name": "Laptop", "price": 15000 }` |

الـ Validation: `Name` مطلوب (حد أقصى 200 حرف)، و `Price` أكبر من 0. الخطأ بيرجع 400 بتفاصيل الحقول.

## الهيكل

```
src/
  ProductsApi.Domain          -> Product (Id, Name, Price)
  ProductsApi.Application     -> Commands / Queries / Handlers / Validators / DTOs / Interfaces
  ProductsApi.Infrastructure  -> EF Core + SQL Server + ProductRepository
  ProductsApi.Api             -> Controller + Program.cs
```

اتجاه الـ dependencies: `Api -> Application -> Domain` و `Infrastructure -> Application -> Domain`.

## الـ CQRS ببساطة

- `CreateProductCommand` بيغيّر داتا، و `GetProductsQuery` / `GetProductByIdQuery` بيقروا بس.
- كل واحد له Handler، والـ `Dispatcher` بيلاقي الـ Handler وبيشغّل الـ Validator قبله.
- من غير MediatR (عشان مبقاش مجاني للاستخدام التجاري من النسخة 13).

## لو عايز Migrations بدل EnsureCreated

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Init -p src/ProductsApi.Infrastructure -s src/ProductsApi.Api
dotnet ef database update -p src/ProductsApi.Infrastructure -s src/ProductsApi.Api
```
(وامسح سطر `EnsureCreatedAsync` من `Program.cs`.)
