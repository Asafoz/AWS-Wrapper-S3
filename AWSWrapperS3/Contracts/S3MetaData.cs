using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSWrapperS3.Contracts
{
    public class S3WrapperObject
    {
        public string BucketName { get; set; }
        public string KeyName { get; set; }
        public MetadataCollection Metadata { get; set; }
    }
}
