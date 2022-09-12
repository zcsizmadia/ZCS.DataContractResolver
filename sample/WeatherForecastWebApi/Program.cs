var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Enable DataContract JSON serializer
        options.JsonSerializerOptions.TypeInfoResolver = System.Text.Json.Serialization.Metadata.DataContractResolver.Default;
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
