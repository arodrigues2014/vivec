namespace Vrt.Vivec.Svc.Clients.Vivec
{
    public class VivecApiClient : HttpClient
    {
        private readonly IConfiguration _configuration;

        public VivecApiClient(IConfiguration configuration)
        {
            _configuration = configuration;

            BaseAddress = new Uri(_configuration.GetValue<string>("Vivec:BaseUrl"));

            Timeout = new TimeSpan(0, 10, 0);

            DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _configuration.GetValue<string>("Payflow:Key"));
        }

        public async Task<string> SendRequest(HttpRequestMessage request)
        {
            var response = await SendAsync(request);
            int tries = 0;

            while (!response.EnsureSuccessStatusCode().IsSuccessStatusCode && tries < 3)
            {
                tries++;
                response = await SendAsync(request);
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
