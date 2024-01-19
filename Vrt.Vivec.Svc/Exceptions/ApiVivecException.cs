namespace Vrt.Vivec.Svc.Exceptions
{
    [Serializable]
    public class ApiVivecException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public DialengaErrorDTO ErrorResponse { get; }

        public ApiVivecException(HttpStatusCode statusCode, DialengaErrorDTO errorResponse, string message)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorResponse = errorResponse;
        }
    }
}
