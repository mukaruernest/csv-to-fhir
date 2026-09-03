# FHIR Patient & Laboratory Data ETL App

A .NET application that reads patient and laboratory data from a CSV file, maps the data to **HL7 FHIR Patient and Observation resources**, and sends them to a FHIR server using the Firely .NET SDK.

## Features

* Reads patient and laboratory data from CSV.
* Maps CSV records to FHIR `Patient` and `Observation` resources.
* Uses the Firely FHIR REST client to communicate with a FHIR server.
* Supports bearer-token authentication.
* Uses dependency injection and configuration-based settings.
* Logs resource creation and errors.

## Project Structure

* `Program.cs` — Configures dependency injection and starts the processing flow.
* `FHIRClientFactory.cs` — Creates and configures the Firely `FhirClient`.
* `FHIRDataService/DataService.cs` — Persists Patient and Observation resources to the FHIR server.
* `Processors/IntegrationService.cs` — Reads the CSV and coordinates resource mapping and persistence.
* `ResourceMappers/PatientMapper.cs` — Maps CSV data to FHIR `Patient` resources.
* `ResourceMappers/ObservationMapper.cs` — Maps CSV data to FHIR `Observation` resources.
* `Models/Models.cs` — Contains the `CSVModel` representing the expected CSV data.

## Configuration

The application uses standard .NET configuration sources.

Important configuration values:

* `Fhir:BaseUrl` — Base URL of the FHIR server.
* `Fhir:BearerToken` — Bearer token for authentication, if required.
* `DataSettings:CsvFilePath` — Path to the CSV file.

Example:

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

Sensitive configuration such as bearer tokens should not be committed to source control. Use environment variables or your deployment platform's secret management for production.

## CSV Format

The CSV should contain patient and laboratory data with columns such as:

```text
SEQN
TIMESTAMP
PATIENT_ID
PATIENT_FAMILYNAME
PATIENT_GIVENNAME
PATIENT_GENDER
WBC
RBC
HB
```

## FHIR Resources

The application creates:

* **Patient** resources from patient information.
* **Observation** resources from laboratory values.

Observations are linked to their corresponding Patient resources.

## Running the Project

From the project directory:

```bash
dotnet run
```

Ensure the FHIR server configuration and CSV file path are configured before running.

## Dependencies

* Firely .NET SDK
* CsvHelper


