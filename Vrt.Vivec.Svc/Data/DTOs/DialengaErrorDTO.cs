namespace Vrt.Vivec.Svc.Data.DTOs
{
    public class DialengaErrorDTO
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("localizedError")]
        public string LocalizedError { get; set; }
    }
}
