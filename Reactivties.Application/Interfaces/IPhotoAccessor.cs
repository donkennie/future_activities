using Microsoft.AspNetCore.Http;
using Reactivties.Application.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactivties.Application.Interfaces
{
    public interface IPhotoAccessor
    {

        Task<PhotoUploadResult> AddPhoto(IFormFile file);

        Task<string> DeletePhoto(string publicId);
    }
}
