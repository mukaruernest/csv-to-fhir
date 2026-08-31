using CDC.Nutrition.Models;
using Hl7.Fhir.Model;

namespace CDC.Nutrition.Resources.ResourceMappers;

public class ObservationMapper
{
    public Observation GetWhiteBloodCells(CSVModel record)
    {
        Observation observation = new Observation
        {
            Code = new CodeableConcept("http://loinc.org", "6690-2", "Leukocytes [#/volume] in Blood by Automated count"),
            Value = new Quantity(decimal.Parse(record.WBC), "10*3/uL", "http://snomed.info/sct"),
            Subject = new ResourceReference($"Patient/{record.PATIENT_ID}"),
            Effective = new FhirDateTime(record.TIMESTAMP),
            Status = ObservationStatus.Final
        };
        observation.Category.Add(new CodeableConcept("http://terminology.hl7.org/CodeSystem/observation-category", "laboratory", "Laboratory"));
        return observation;
    }

    public Observation GetRedBloodCells(CSVModel record)
    {
        Observation observation = new Observation
        {
            Code = new CodeableConcept("http://loinc.org", "6690-2", "Leukocytes [#/volume] in Blood by Automated count"),
            Value = new Quantity(decimal.Parse(record.RBC), "10*3/uL", "http://snomed.info/sct"),
            Subject = new ResourceReference($"Patient/{record.PATIENT_ID}"),
            Effective = new FhirDateTime(record.TIMESTAMP),
            Status = ObservationStatus.Final
        };
        observation.Category.Add(new CodeableConcept("http://terminology.hl7.org/CodeSystem/observation-category", "laboratory", "Laboratory"));
        return observation;
    }

    public Observation GetHemoglobin(CSVModel record)
    {
        Observation observation = new Observation
        {
            Code = new CodeableConcept("http://loinc.org", "718-7", "Hemoglobin [Mass/volume] in Blood"),
            Value = new Quantity(decimal.Parse(record.HB), "g/dL", "http://snomed.info/sct"),
            Subject = new ResourceReference($"Patient/{record.PATIENT_ID}"),
            Effective = new FhirDateTime(record.TIMESTAMP),
            Status = ObservationStatus.Final
        };
        observation.Category.Add(new CodeableConcept("http://terminology.hl7.org/CodeSystem/observation-category", "laboratory", "Laboratory"));
        return observation;
    }
}