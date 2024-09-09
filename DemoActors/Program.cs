using DemoActors.Classes;
using Dapr.Actors;
using Dapr.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<ShoppingCartActor>();
    options.Actors.RegisterActor<ReminderActor>();
    options.Actors.RegisterActor<TimerActor>();
});

var app = builder.Build();

// Enable Dapr Actors
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapActorsHandlers();
});

app.Run();
