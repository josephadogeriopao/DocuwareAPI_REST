var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 1. Register SwaggerGen (Swashbuckle) instead of AddOpenApi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 2. Enable the classic Swashbuckle JSON generator
    app.UseSwagger();

    // 3. Point UI to the Swashbuckle JSON endpoint (standard path)
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
