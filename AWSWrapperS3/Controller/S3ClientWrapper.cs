using Amazon;
using Amazon.S3;
using AWSWrapperS3.Contracts;
using AWSWrapperS3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSWrapperS3.Controller
{
    public class S3ClientWrapper
    {
        //private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;

        ////public static void Main()
        ////{
        ////    s3Client = new AmazonS3Client(bucketRegion);
        ////    UploadFileAsync().Wait();
        ////}

        //private static async Task UploadFileAsync(IAmazonS3 s3Client

        protected IAmazonS3 S3Client { get; set; }

        public S3ClientWrapper(RegionEndpoint bucketRegion)
        {
            S3Client = new AmazonS3Client(bucketRegion);
        }
        public static void UploadObjectUsingPresignedURL(string url, string filePath, IProgress<FileTransferProgress> progress)
        {
            UploadFileAPI.UploadObjectUsingPresignedURL(url, filePath , progress);
        }

        public async Task UploadFileAsync(string filePath, S3WrapperObject s3Object)
        {
            await UploadFileAPI.UploadFileAsync(S3Client, filePath, s3Object);
        }
    }
}
