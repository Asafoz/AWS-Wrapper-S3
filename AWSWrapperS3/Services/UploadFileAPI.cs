using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using AWSWrapperS3.Contracts;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AWSWrapperS3.Services
{
    //https://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadFileDotNet.html
    internal class UploadFileAPI
    {

        //private const string filePath = "*** provide the full path name of the file to upload ***";
        // Specify your bucket region (an example region is shown).

        //public static void Main()
        //{
        //    s3Client = new AmazonS3Client(bucketRegion);
        //    UploadFileAsync().Wait();
        //}

        internal static async Task UploadFileAsync(IAmazonS3 s3Client, string filePath,S3WrapperObject s3Object)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                //// Option 1. Upload a file. The file name is used as the object key name.
                //await fileTransferUtility.UploadAsync(filePath, s3Location.BucketName);
                //Console.WriteLine("Upload 1 completed");

                //// Option 2. Specify object key name explicitly.
                //await fileTransferUtility.UploadAsync(filePath, s3Location.BucketName, s3Location.KeyName);
                //Console.WriteLine("Upload 2 completed");

                //// Option 3. Upload data from a type of System.IO.Stream.
                //using (var fileToUpload =
                //    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //{
                //    await fileTransferUtility.UploadAsync(fileToUpload,
                //                               s3Location.BucketName, s3Location.KeyName);
                //}
                //Console.WriteLine("Upload 3 completed");

                // Option 4. Specify advanced settings.
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = s3Object.BucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.
                    Key = s3Object.KeyName,
                    CannedACL = S3CannedACL.PublicRead
                };

                foreach (var item in s3Object.Metadata.Keys)
                {
                    fileTransferUtilityRequest.Metadata.Add(item, s3Object.Metadata[item]);
                }

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }

        internal static void UploadObjectUsingPresignedURL(string url, string filePath, IProgress<FileTransferProgress> progress)
        {
            HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "PUT";

            int nOffset = 0;
            long lFileSize = new System.IO.FileInfo(filePath).Length;

            using (Stream dataStream = httpRequest.GetRequestStream())
            {
                var buffer = new byte[8000];
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dataStream.Write(buffer, 0, bytesRead);
                        nOffset += bytesRead;
                        progress.Report(new FileTransferProgress(nOffset, lFileSize));
                    }
                }
            }
            HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
        }
    }
}
