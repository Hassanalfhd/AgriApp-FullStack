using Azure.Core;

namespace Agricultural_For_CV.Helpers
{
    public class UrlHelper
    {

        public static string?GetFullUrl(string baseUrl, string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;
            
            return $"{baseUrl}{relativePath}";

        }
    }
}
