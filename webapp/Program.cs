using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Interfaces;
using webapp.Repository;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();


app.MapGet("/api", () => "Hello World!");

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.Now}] Started.");
    await next();
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.Now}] Finished.");
});
app.MapControllers();

app.Run();
