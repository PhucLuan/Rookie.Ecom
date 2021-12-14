using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Rookie.Ecom.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Services
{
    public class CloudinaryService : ICloudinaryService
    {

        public CloudinaryService() { }

        public async Task<string> UploadImageByStream(Account account, string name, Stream stream, string publicId)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(name, stream),
                PublicId = publicId
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            //return uploadResult.SecureUrl.AbsoluteUri;
            //uploadResult.Result.SecureUrl.AbsoluteUri
            return uploadResult.SecureUrl.AbsoluteUri;

        }
        public async Task<DeletionResult> RemoveImage(Account account, string publicId)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
            DeletionParams destroyParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            return await cloudinary.DestroyAsync(destroyParams);
        }

        public async Task<DelResResult> RemoveListImage(Account account, string[] public_ids)
        {
            if (public_ids.Count() > 0)
            {
                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;
                var delResParams = new DelResParams()
                {
                    PublicIds = public_ids.ToList()
                };
                return await cloudinary.DeleteResourcesAsync(delResParams);
            }
            return null;
        }
    }
}
