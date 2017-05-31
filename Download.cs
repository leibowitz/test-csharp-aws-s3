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
				Console.WriteLine("Retrieving (GET) an object");
				ReadObjectData(client);
				//Task<string> content = ReadObjectData(client);
				//Console.WriteLine(content.Result);
			}
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}


		//static async Task<string> ReadObjectData(IAmazonS3 client)
		static async Task ReadObjectData(IAmazonS3 client)
		{
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

					}
			}
			catch (AmazonS3Exception s3Exception)
			{
				Console.WriteLine(s3Exception.Message,
						s3Exception.InnerException);
			}
		}

	}
}
