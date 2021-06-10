using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dynamsoft.DBR;
using Dynamsoft;
using System.Net.Http;
using System.Text;
using System.Drawing;

namespace BlazorTWAIN_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadBarcodesController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        public ReadBarcodesController(IWebHostEnvironment env) => this.env = env;

        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            /* Note:
                This demo shows how to save a file to the wwwroot folder.
                1. wwwroot must be present in the application or WebRootPath will return null.
                2. This is for demo purposes, use caution when providing users with the ability
                    to upload files to a server. 
                    See: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0#security-considerations
            */

            // Create a BadRequestResult (400 Bad Request) when data is missing.
            if (file == null || file.Length == 0)
            {
                return new BadRequestResult();
            }
            // Ex. path: "wwwroot/uploads/filename.jpg"
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            //string path = Path.Combine(env.WebRootPath, "uploads", file.FileName);
            //string dirPath = Path.Combine(env.WebRootPath, "uploads"); save file if you need. 
            //if (System.IO.File.Exists(dirPath) == false)
            //{
            //    Directory.CreateDirectory(dirPath);
            //}
            //await Task.WhenAll(
            //    file.CopyToAsync(ms),
            //    System.IO.File.WriteAllBytesAsync(path, ms.ToArray())
            //);
            using MemoryStream ms = new();
            await file.CopyToAsync(ms);
            BarcodeReader reader = new BarcodeReader("t0069fQAAAAM656kb3qDzrc9Yf4UYDqMekf2RajobBUxz7AB6+lUHifyq/jKD47/HS94EHH6F2prBZqFlU0W4rq4r1feL3j92");            
            TextResult[] results = reader.DecodeFileInMemory(ms.ToArray(), "");
            string jsonString = System.Text.Json.JsonSerializer.Serialize(ConvertedResult(results));
            return Content(jsonString);
        }

        private List<Data.TextResult> ConvertedResult(TextResult[] results)
        {
            List<Data.TextResult> newResults = new List<Data.TextResult>();
            foreach (TextResult result in results)
            {
                Data.TextResult newResult = new Data.TextResult();
                newResult.text = result.BarcodeText;
                newResult.note = result.BarcodeFormatString;
                Point[] resultPoints = result.LocalizationResult.ResultPoints;
                newResult.x1 = resultPoints[0].X;
                newResult.x2 = resultPoints[1].X;
                newResult.x3 = resultPoints[2].X;
                newResult.x4 = resultPoints[3].X;
                newResult.y1 = resultPoints[0].Y;
                newResult.y2 = resultPoints[1].Y;
                newResult.y3 = resultPoints[2].Y;
                newResult.y4 = resultPoints[3].Y;
                newResults.Add(newResult);
            }
            return newResults;
        }
    }
}
