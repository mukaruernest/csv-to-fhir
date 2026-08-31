using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;

namespace CDC.Nutrition.FHIRDataService;

public class DataService(ILogger<DataService> logger, FhirClientFactory fhirClientFactory)
{
    private readonly FhirClient client = fhirClientFactory.Create();
    public async System.Threading.Tasks.Task StoreDataAsync(List<Patient> patients, List<Observation> observations)
    {
        // Code to add patient to FHIR server
        foreach (var patient in patients)
        {
            try
            {
                Patient createdPatient = null;
                createdPatient = await client.CreateAsync(patient);
                logger.LogInformation($"Created patient with ID: {createdPatient.Id}");

                var patientObservations = observations.FindAll(observation => observation.Subject.Reference == $"Patient/{patient.Id}").ToList();
            
                foreach (var obs in patientObservations)
                {
                    obs.Subject = new ResourceReference($"Patient/{createdPatient.Id}");
                    var createdObservation = await client.CreateAsync(obs);
                    logger.LogInformation($"Created observation with ID: {createdObservation.Id} for patient ID: {createdPatient.Id}");
                }   

                if (createdPatient != null)
                {
                    Bundle record = await client.FetchPatientRecordAsync(createdPatient.ResourceIdentity());
                    logger.LogInformation($"The record contains {record.Entry.Count} resources, the first of which is the Patient");
                } 
            }
            catch (FhirOperationException ex)
            {
                logger.LogError($"Error creating patient: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected error: {ex.Message}");
            }
        }
    }
}

