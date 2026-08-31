using Hl7.Fhir.Rest;
using System.Net.Http.Headers;

public class FhirClientFactory(IConfiguration configuration)
{

    public FhirClient Create()
    {
        var baseUrl = configuration["Fhir:BaseUrl"];
        var token = configuration["Fhir:BearerToken"];

        var client = new FhirClient(baseUrl);

        client.RequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return client;
    }
}