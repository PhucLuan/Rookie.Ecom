using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageByStream(Account account,string name, Stream stream, string publicId);
        Task<DeletionResult> RemoveImage(Account account, string publicId);
        Task<DelResResult> RemoveListImage(Account account, string[] public_ids);
    }
}
