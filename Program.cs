using System;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace ConsoleApplication
{
	public class Program
	{

		static string bucketName = "something2409840918";
		static string keyName    = "blabla";
		static IAmazonS3 client;

		public static void Main(string[] args)
		{
			Console.WriteLine("creating client");
			using (client = new AmazonS3Client("", "", Amazon.RegionEndpoint.EUWest2)) 
			{
				Console.WriteLine("Saving (PUT) an object");
				//Task<PutObjectResponse> tres = PutObjectData(client, "blabla");
				// https://msdn.microsoft.com/en-us/library/system.net.httpstatuscode.aspx
				//Console.WriteLine("Response: {0}", tres.Result.HttpStatusCode);
				PutObjectData(client, "blabla");
				Console.WriteLine("Done");
				/*Console.WriteLine("Retrieving (GET) an object");
				Task<string> content = ReadObjectData(client);
				Console.WriteLine(content.Result);*/
			}
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		// http://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/S3/TS3PutObjectResponse.html
		//static async Task<PutObjectResponse> PutObjectData(IAmazonS3 client, string file)
		static void PutObjectData(IAmazonS3 client, string file)
{

TransferUtility fileTransferUtility =
     new TransferUtility(client);
TransferUtilityUploadRequest uploadRequest =
    new TransferUtilityUploadRequest
    {
	BucketName = bucketName,
	Key = keyName,
    	FilePath = file 
    };

uploadRequest.UploadProgressEvent +=
    new EventHandler<UploadProgressArgs>
        (uploadRequest_UploadPartProgressEvent);


fileTransferUtility.Upload(uploadRequest);
}

		static async Task<string> ReadObjectData(IAmazonS3 client)
		{
			string responseBody = "";

			GetObjectRequest request = new GetObjectRequest 
			{
				BucketName = bucketName,
					   Key = keyName
			};

			try
			{
				Console.WriteLine("doing request");
				using (GetObjectResponse response = await client.GetObjectAsync(request))  
					using (Stream responseStream = response.ResponseStream)
					using (StreamReader reader = new StreamReader(responseStream))
					{
						string title = response.Metadata["x-amz-meta-title"];
						Console.WriteLine("The object's title is {0}", title);

						responseBody = reader.ReadToEnd();
					}
			}
			catch (AmazonS3Exception s3Exception)
			{
				Console.WriteLine(s3Exception.Message,
						s3Exception.InnerException);
			}

			return responseBody;
		}

static void uploadRequest_UploadPartProgressEvent(object sender, UploadProgressArgs e)
{    
    Console.WriteLine("{0}/{1} {2}%", e.TransferredBytes, e.TotalBytes, e.PercentDone);
}

	}
}
