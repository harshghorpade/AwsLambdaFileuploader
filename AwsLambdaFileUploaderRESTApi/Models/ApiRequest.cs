// ==================================================
// API Request Model
// ==================================================

using Microsoft.AspNetCore.Http;

namespace AwsLambdaFileUploaderRESTApi.Models
{
    public class fileInfoObject
    {
        public string fileType { get; set; }
        public IFormFile file { get; set; }
    }
}
