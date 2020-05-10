# AwsLambdaFileuploader
AWS Lambda file uploader demonstrates how to use server-less framework to upload small size files (up to 5mb) to cloud. * Large size files needs special treatment, uploading via S3 signed URLs which is not discussed in this topic.

## Prerequisite
1] AWS account
2] Visual Studio 2019 with .NET Core 3.1 installed
3] AWS Toolkit for Visual Studio 2019 (* for easy cloud deployment of serverless web API from within Visual Studio) and AWS account profile setup in Visual Studio.

## Steps to setup for local debugging
1] Clone the repository and open the solution file in Visual Studio 2019
2] For local debugging you may need to create “tmp” folder on specified path in code.
3] Make the required code changes, like updating proper S3 bucket name (which you need to create separately in us-east-1 region using your AWS account)
4] For local debugging start the debugger from Visual Studio (by hitting F5 key)
5] And from Postman hit the following URL [POST http://localhost:5000/api/upload/fileWithBodyParam]
6] Need to pass file using Postman body “form-data” tab and putting key as “file” and then selecting type as “file” so that file chooser will appear in value tab and hit send
7] On successful upload response will get “File Upload Complete”
8] On Failure response will get “File Upload Failed”

## Steps for cloud deployment
1] Once the local debugging is successful then right click on project in Visual Studio and select “Publish to AWS Lambda” deployment option and follow the steps.
2] On Successful deployment of server-less application you will get the prod stage API Gateway link.
3] Before using this link we need to make small change in API Gateway configuration. Change the “Binary Media Types” in API Gateway settings to “multipart/form-data”. This is required to tell AWS API Gateway not to encode the form-data (which is send file contents) differently, treat it as binary media data and pass as it is.
4] Now setup the Postman POST requires that we created above to point to deployed lambda (API Gateway link that received in step 2) and hit send.
5] On successful upload response your sent file should get uploaded to specified S3 bucket using server-less framework (API Gateway + Lambda).

