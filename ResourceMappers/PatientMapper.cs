using CDC.Nutrition.Models;
using Hl7.Fhir.Model;
namespace CDC.Nutrition.Resources.ResourceMappers;

public class PatientMapper
{
    public Patient GetPatient(CSVModel record)
    {
        var patient = new Patient();

        patient.Id = record.PATIENT_ID;
        patient.Name.Add(new HumanName
        {
            Use = HumanName.NameUse.Official,
            Family = record.PATIENT_FAMILYNAME,
            Given = new List<string> { record.PATIENT_GIVENNAME }
        });
        patient.Gender = record.PATIENT_GENDER == "M" ? AdministrativeGender.Male : AdministrativeGender.Female;
        return patient;
    }
}
