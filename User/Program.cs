using Microsoft.OpenApi.Models;
using User.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDataAccess(builder.Configuration);


//Swagger Documentation Section
var info = new OpenApiInfo()
{
    Title = "User Management Service API Documentation",
    Version = "v1",
    Description = "API respresents the user management micro application. " +
        "It provides the endpoints to get and manage schools, users, parents of students.",
    Contact = new OpenApiContact()
    {
        Name = "Daniil Zharikov",
        Email = "dszharikov@yandex.ru",
    }

};

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "V1");
        });
}

app.MapControllers();

app.MapGet("/", () => "Hello World! User Management Service API is running!");

app.Run();
