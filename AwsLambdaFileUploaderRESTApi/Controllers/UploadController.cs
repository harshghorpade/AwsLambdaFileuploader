// ==================================================
// Upload API Controller
// ==================================================

using System;
using System.IO;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AwsLambdaFileUploaderRESTApi.Models;

namespace AwsLambdaFileUploaderRESTApi.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private static readonly Amazon.RegionEndpoint bucketRegion = Amazon.RegionEndpoint.USEast1;
        private const string bucketName = "*** provide S3 bucket name ***";    // create bucket in specified region and put that name here
        private static IAmazonS3 s3Client;

        public UploadController()
        {
            s3Client = new AmazonS3Client(bucketRegion);
        }

        // POST api/upload/file
        [HttpPost]
        [Route("file")]
        public async Task<IActionResult> FileUploader(IFormFile file)
        {
            Console.WriteLine($"File upload start for : {file.FileName}");
            try
            {
                string inputFileName = Path.GetFileName(file.FileName);
                string inputFilePath = "/tmp/" + inputFileName;     // storing the file in Lambda temporary storage (current limit is 512 MB)

                using (var fs = new FileStream(inputFilePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                    fs.Close();

                    // now send recieved file to S3 bucket
                    var fileTransferUtility = new TransferUtility(s3Client);
                    await fileTransferUtility.UploadAsync(inputFilePath, bucketName);

                    Console.WriteLine($"file upload complete for {inputFileName} : {file.Length} bytes");
                }
            }
            catch(Exception exp)
            {
                Console.WriteLine($"ERROR : file upload failed for {file.FileName} due to : {exp.Message}");
                return new OkObjectResult($"File upload failed");
            }
            return new OkObjectResult($"File Upload Complete");
        }

        [HttpPost]
        [Route("fileWithBodyParam")]
        public async Task<IActionResult> FileUploaderWithBodyParam([FromForm] fileInfoObject fileInformation)
        {
            IFormFile inputFile = fileInformation.file;
            string fileType = fileInformation.fileType;
            Console.WriteLine($"File upload start for : {inputFile.FileName} of type {fileType}");

            try
            {
                string inputFileName = Path.GetFileName(inputFile.FileName);
                string inputFilePath = "/tmp/" + inputFileName;     // storing the file in Lambda temporary storage (current limit is 512 MB)

                using (var fs = new FileStream(inputFilePath, FileMode.Create))
                {
                    inputFile.CopyTo(fs);
                    fs.Close();

                    // now send recieved file to S3 bucket
                    var fileTransferUtility = new TransferUtility(s3Client);
                    await fileTransferUtility.UploadAsync(inputFilePath, bucketName);

                    Console.WriteLine($"file upload complete for {inputFileName} : {inputFile.Length} bytes");
                }
            }
            catch(Exception exp)
            {
                Console.WriteLine($"ERROR : file upload failed for {inputFile.FileName} due to : {exp.Message}");
                return new OkObjectResult($"File upload failed");
            }
            return new OkObjectResult($"File Upload Complete");
        }
    }
}
