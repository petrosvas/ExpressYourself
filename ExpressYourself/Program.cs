using ExpressYourself.Adapters;
using ExpressYourself.Entity_Framework.DBContext;
using ExpressYourself.Entity_Framework.Implementations;
using ExpressYourself.Entity_Framework.Interfaces;
using ExpressYourself.Implementations;
using ExpressYourself.Interfaces;
using ExpressYourself.Types;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<AppDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IExpressYourselfService, ExpressYourselfService>();
builder.Services.AddSingleton<ICachingManager, CachingManager>();
builder.Services.Decorate<ICachingManager, CachingManagerDecorator>();
builder.Services.AddSingleton<IDBManager, DBManager>();
builder.Services.AddSingleton<IDBAdapter, DBAdapter>();
builder.Services.AddScoped<IEFManager, EFManager>();
builder.Services.AddScoped<IEFManagerAdapter, EFManagerAdapter>();
builder.Services.AddSingleton<IHTTPManager, HTTPManager>();
builder.Services.Decorate<IHTTPManager, HTTPManagerDecorator>();
builder.Services.AddHostedService<UpdateIPs>();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();