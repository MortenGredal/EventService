using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;
using EventService.DbContexts;
using EventService.Models;
using EventService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContexts
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("OrdersConnection")));
builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EventsConnection")));

// Create Channel
var pizzaOrderChannel =
    Channel.CreateUnbounded<PizzaOrder>(); // create unbounded, because we're not running prod stuff, warning, will eat your ram if spammed


// Register ChannelReader and ChannelWriter separately
builder.Services.AddSingleton<ChannelWriter<PizzaOrder>>(_ => pizzaOrderChannel.Writer);
builder.Services.AddSingleton<ChannelReader<PizzaOrder>>(_ => pizzaOrderChannel.Reader);

// Register Services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<EventService.Services.EventService>();
builder.Services.AddHostedService<EventProcessingService>();

// Configure logging for Host, as Hosted Services do not have logging enabled by default
builder.Host.ConfigureLogging((_, logging) => { logging.AddConsole(); });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Ensure databases are created
using (var scope = app.Services.CreateScope())
{
    var orderContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    var eventContext = scope.ServiceProvider.GetRequiredService<EventDbContext>();

    orderContext.Database.EnsureCreated();
    eventContext.Database.EnsureCreated();
}

app.Run();