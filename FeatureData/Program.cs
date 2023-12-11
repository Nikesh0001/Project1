using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var azureTableStorageSettings = builder.Configuration.GetSection("AzureTableStorageSettings");
string connectionString = builder.Configuration.GetConnectionString("constring");
string tableName = azureTableStorageSettings.GetValue<string>("TableName");


builder.Services.AddScoped<IFeatureRepository, FeatureRepository>(provider =>
{
    return new FeatureRepository(connectionString, tableName);
});
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:constring:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:constring:queue"], preferMsi: true);
});


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
