using CDC.Nutrition.FHIRDataService;
using CDC.Nutrition.Models;
using CDC.Nutrition.Resources.ResourceMappers;
using CsvHelper;
using Hl7.Fhir.Model;

namespace CDC.Nutrition.Processors;
public class IntegrationService(DataService dataService, ILogger<IntegrationService> logger)
{
    public async System.Threading.Tasks.Task MapResources(string csvFilePath)
    {
        logger.LogInformation("Starting CSV processing...");
        using var reader = new StreamReader(csvFilePath);
        using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        var records = csv.GetRecords<CSVModel>().ToList();
        var patients = new List<Patient>();
        var observation = new List<Observation>();
        var map = new PatientMapper();
        var observationMap = new ObservationMapper();

        foreach (var record in records)
        {
            var patient = map.GetPatient(record);
            if (!patients.Any(p => p.Id == patient.Id))
            patients.Add(patient);
            
            observation.Add(observationMap.GetWhiteBloodCells(record));
            observation.Add(observationMap.GetRedBloodCells(record));
            observation.Add(observationMap.GetHemoglobin(record));
        }
        await dataService.StoreDataAsync(patients, observation);
        logger.LogInformation("Finished processing CSV and storing data.");
    }
}

