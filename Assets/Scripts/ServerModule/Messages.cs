static public class Messages {
    public const string preRM = "[server module -> RequestMan] ";
    public const string preRQ = "[server module -> Request] ";
    public const string preRS = "[server module -> Response] ";
    public const string preCT = "[server module -> Certificate] ";
    public const string preTR = "[server module -> TestREST] ";
    public const string MutexTimeout = "Couldn't aquire mutex, because it timed out (Request might've locked the mutex).";
    public const string CertificateOk = "Received certificate is trusted.";
    public const string CertificateNotOk = "Certificate is NOT trusted.";
    public const string SendRequestOk = "Request sent.";
    public const string SendRequestNotOk = "Request failed.";
    public const string GetResponseBadId = "Provided id does not belong to any active requests.";
    public const string GetResponseNotFound = "Response not yet completed. Returning null.";
    public const string GetResponseOk = "Response ok. Destroying request.";
}
