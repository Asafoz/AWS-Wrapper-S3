using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSWrapperS3.Contracts
{
    public class FileTransferProgress
    {
        public FileTransferProgress(long nOffset, long lFileSize) 
        {
            Offset = nOffset;
            FileSize = lFileSize;
        }

        public long Offset { get; set; }
        public long FileSize { get; set; }
    }
}
