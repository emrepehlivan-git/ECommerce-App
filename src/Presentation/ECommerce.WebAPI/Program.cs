using ECommerce.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.UsePresentation(app.Environment);

app.Run();