using BlazorTWAIN.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace BlazorTWAIN.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCRController : ControllerBase
    {
        private OcrEngine ocrEngine;
        public OCRController()
        {
            ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
        }

        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new BadRequestResult();
            }
            using MemoryStream ms = new();
            await file.CopyToAsync(ms);
            byte[] bytes = ms.ToArray();
            IBuffer buffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);
            InMemoryRandomAccessStream inStream = new InMemoryRandomAccessStream();
            DataWriter datawriter = new DataWriter(inStream.GetOutputStreamAt(0));
            datawriter.WriteBuffer(buffer, 0, buffer.Length);
            await datawriter.StoreAsync();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(inStream);
            SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            OcrResult ocrResult = await ocrEngine.RecognizeAsync(bitmap);
            string jsonString = System.Text.Json.JsonSerializer.Serialize(ConvertedResult(ocrResult));
            return Content(jsonString);
        }

        private List<TextResult> ConvertedResult(OcrResult ocrResult)
        {
            List<TextResult> newResults = new List<TextResult>();
            foreach (OcrLine line in ocrResult.Lines)
            {
                TextResult newResult = new TextResult();
                int X, Y, Right, Bottom;
                X = (int)line.Words[0].BoundingRect.X;
                Y = (int)line.Words[0].BoundingRect.Y;
                Right = (int)line.Words[0].BoundingRect.Right;
                Bottom = (int)line.Words[0].BoundingRect.Bottom;
                foreach (OcrWord word in line.Words)
                {
                    Rect rect = word.BoundingRect;
                    newResult.text += word.Text + " ";
                    X = (int)Math.Min(rect.X, X);
                    Y = (int)Math.Min(rect.Y, Y);
                    Right = (int)Math.Max(rect.Right, Right);
                    Bottom = (int)Math.Max(rect.Bottom, Bottom);
                }
                newResult.x1 = X;
                newResult.x2 = Right;
                newResult.x3 = Right;
                newResult.x4 = X;
                newResult.y1 = Y;
                newResult.y2 = Y;
                newResult.y3 = Bottom;
                newResult.y4 = Bottom;
                newResults.Add(newResult);
            }
            return newResults;
        }
    }
}
