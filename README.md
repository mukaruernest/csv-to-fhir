# CSV to FHIR Resource Mapper

This project reads a CSV of patient/lab data, maps the rows to FHIR resources (Patient and Observation), and writes those resources to a FHIR server.

Key features
- Reads a CSV file containing patient and laboratory values.
- Maps CSV rows to FHIR Patient and Observation resources using small mapper classes.
- Uses an Hl7.Fhir REST client to create Patient and Observation resources on a configured FHIR server.
- Basic logging and error handling during resource creation.

Project structure (important files)
- Program.cs
  - Configures dependency injection and starts the small processing flow. The CSV file path is taken from configuration (DataSettings:CsvFilePath) and then IntegrationService.MapResources is invoked.

- FHIRClientFactory.cs
  - Creates and configures an Hl7.Fhir.Rest.FhirClient using configuration keys Fhir:BaseUrl and Fhir:BearerToken.

- FHIRDataService/DataService.cs
  - Responsible for persisting mapped resources to the FHIR server. For each Patient it:
	1) Creates the Patient on the FHIR server.
	2) Finds any Observation objects that referenced the original Patient id and rebinds them to the created patient's id.
	3) Creates each Observation on the FHIR server.
	4) Fetches the patient record bundle for verification/logging.

- Processors/IntegrationService.cs
  - Reads the CSV (CsvHelper), maps rows to domain model CSVModel, creates FHIR resources using the mappers, and calls DataService to store them.

- ResourceMappers/PatientMapper.cs
- ResourceMappers/ObservationMapper.cs
  - Map CSVModel instances to Hl7.Fhir.Model.Patient and Hl7.Fhir.Model.Observation respectively.

- Models/Models.cs
  - Contains CSVModel which defines the CSV columns expected (SEQN, TIMESTAMP, PATIENT_ID, PATIENT_FAMILYNAME, PATIENT_GIVENNAME, PATIENT_GENDER, WBC, RBC, HB).

Configuration
- The app expects configuration values in the standard .NET configuration sources (appsettings.json / environment variables). The important keys are:
  - Fhir:BaseUrl - base URL of your FHIR server (e.g. https://fhir.example.org)
  - Fhir:BearerToken - bearer token for authentication (if required by your FHIR server)
  - DataSettings:CsvFilePath - full path to the CSV file to process

Example appsettings.json snippet

```json
{
  "Fhir": {
	"BaseUrl": "https://fhir.example.org",
	"BearerToken": "<token>"
  },
  "DataSettings": {
	"CsvFilePath": "C:/data/patients.csv"
  }
}
```

CSV format
- The CSV should map to the CSVModel properties. Minimal required columns: SEQN, PATIENT_ID, PATIENT_FAMILYNAME, PATIENT_GIVENNAME. TIMESTAMP should be parseable by FhirDateTime.

Running the project
- This is a minimal console/web-hosted app. From the project directory:

- dotnet run

Behavior notes and caveats
- The code currently parses numeric observation values with decimal.Parse; malformed or empty values will throw — consider using TryParse or defensive checks.
- Observations are created after the Patient is created; the mapper initially sets Subject to Patient/{PATIENT_ID} and DataService rebases that to the created resource id.
- Duplicate patient rows are de-duplicated in memory using the Patient.Id value before sending to the FHIR server.
- Basic exception handling logs FhirOperationException and general Exception when creating resources.

Dependencies
- Hl7.Fhir.* packages (for FHIR model and REST client)
- CsvHelper (for CSV parsing)

Improvements being worked on
- Add robust validation for CSV values and better error reporting per-row.
- Handle partial failures (e.g., continue processing other patients when one patient fails) with configurable retry/backoff.
- Support bulk transactions / bundled creates to reduce HTTP calls to the FHIR server.


