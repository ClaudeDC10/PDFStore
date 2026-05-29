using PDFStore.Api.Extensions;
using PDFStore.Core.Interfaces;
using PDFStore.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPdfStoreData(builder.Configuration);
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IRetrievalService, RetrievalService>();
builder.Services.AddTransient<IPdfReaderService, PdfReaderService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
