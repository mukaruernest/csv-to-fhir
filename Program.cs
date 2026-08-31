using CDC.Nutrition.FHIRDataService;
using CDC.Nutrition.Processors;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<IntegrationService>();
builder.Services.AddScoped<FhirClientFactory>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var patientResource = scope.ServiceProvider.GetRequiredService<IntegrationService>();
    string dataPath = app.Configuration["DataSettings:CsvFilePath"] 
                      ?? throw new InvalidOperationException("CSV path not configured.");
    await patientResource.MapResources(dataPath);
}
app.Run();



