using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Api.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ipstset.AzShop.Api.Files.Images
{
    [Route("files/{shopId}/images")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [HttpException]
    public class FilesImagesController: BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FilesImagesController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpGet(Name = "GetImageFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] string shopId)
        {
            return Ok("works");
        }

        [HttpPost(Name="CreateImageFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromRoute] string shopId, [FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            //todo: validate extension, shopid
            var formattedShopId = Guid.Parse(shopId).ToString("N");

            var result = await WriteFile(file, formattedShopId);

            if (result.IsError)
                throw new Exception(result.ErrorMessage);

            return Ok(new { images = new List<dynamic>
                {
                    new { Url = $".../content/shops/{formattedShopId}/{result.FileName}", shopId}
                }
           });

        }

        private async Task<FileUploadResult> WriteFile(IFormFile file, string shopId)
        {
            var result = new FileUploadResult();
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileName = DateTime.Now.Ticks + extension;

                var pathBuilt = Path.Combine(_hostingEnvironment.WebRootPath, $"content/shops/{shopId}");

                if (!Directory.Exists(pathBuilt))
                    Directory.CreateDirectory(pathBuilt);

                var path = Path.Combine(pathBuilt, fileName);
                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                result.FileName = fileName;
            }
            catch (Exception ex)
            { 
                result.IsError = true;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private struct FileUploadResult
        {
            public string FileName { get; set; }
            public bool IsError { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
