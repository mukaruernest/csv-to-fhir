using System.Collections.Generic;

namespace CDC.Nutrition.Models
{
    public class CSVModel
    {
        public required string SEQN { get; set; }
        public string? TIMESTAMP { get; set; }
        public required string PATIENT_ID { get; set; }
        public string? PATIENT_FAMILYNAME { get; set; }
        public string? PATIENT_GIVENNAME { get; set; }
        public string? PATIENT_GENDER { get; set; }
        public string? WBC { get; set; }
        public string? RBC { get; set; }
        public string? HB { get; set; }
    }
}
