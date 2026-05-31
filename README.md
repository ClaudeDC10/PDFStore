# PDFStore

An ASP.NET Core Web API for uploading PDF files, extracting their text content via PdfPig, and storing them in a SQLite database for later retrieval.

## Projects

| Project | Purpose |
|---|---|
| `PDFStore.Api` | Web API layer — controllers, DI setup, entry point |
| `PDFStore.Core` | Business logic — services and interfaces |
| `PDFStore.Infrastructure` | Data access layer — EF Core DbContext and repositories |

## Tech Stack

- .NET 10
- ASP.NET Core
- Entity Framework Core + SQLite
- PdfPig (PDF text extraction)

## API Endpoints

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/documents/upload` | Upload a PDF file |
| `GET` | `/api/documents/retrieve/{id}` | Retrieve a document by GUID |
| `GET` | `/api/documents/retrieve/all` | Retrieve all documents (optional `?limit=N`) |
| `GET` | `/api/documents/retrieve/all/{fileName}` | Retrieve all documents by file name (optional `?limit=N`) |

## Document Response Shape

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fileName": "example.pdf",
  "content": "Extracted text content..."
}
```

## EF Core Migrations
The SQLite database (`pdfstore.db`) is created automatically on first run via EF Core migrations.

*Install EF Core Tools*
```pwsh
dotnet tool install --global dotnet-ef
```

*Generate Migration files*
```pwsh
dotnet ef migrations add InitialCreate --project PDFStore.Infrastructure --startup-project PDFStore.Api
```

*Create database*
```pwsh
dotnet ef database update --project PDFStore.Infrastructure --startup-project PDFStore.Api
```

## Getting Started

1. Clone the repo
2. Run the API project:
   ```
   dotnet run --project PDFStore.Api
   ```
3. Upload a PDF using the `/api/documents/upload` endpoint with a `pdf` form field