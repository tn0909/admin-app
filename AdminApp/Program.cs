using System.Text.Json.Serialization;
using AdminApp.Extensions;
using AdminApp.Profiles;
using AdminApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Elasticsearch
builder.Services.AddElasticSearch(builder.Configuration);
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
    .AddControllers()
    .AddJsonOptions(o => 
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
    o.AddPolicy("CorsPolicy", builder => 
        builder.WithOrigins("http://localhost:4200") // Update with your Angular app URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("CorsPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
