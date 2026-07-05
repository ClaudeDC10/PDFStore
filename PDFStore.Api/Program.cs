using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi;
using PDFStore.Api.Extensions;
using PDFStore.Core.Interfaces;
using PDFStore.Core.Services;
using PDFStore.Infrastructure.Services;
using PDFStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPdfStoreData(builder.Configuration);
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IRetrievalService, RetrievalService>();
builder.Services.AddTransient<IPdfReaderAdapter, PdfReaderAdapter>();

builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PDFStoreAPI", Version = "v1" } );
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DocumentContext>();
    await context.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
internal partial class Program { }