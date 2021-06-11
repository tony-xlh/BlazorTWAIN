using Dynamsoft.DBR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace BlazorTWAIN_Server.Data
{
    public class OCRResultService
    {
        private OcrEngine ocrEngine;
        public OCRResultService() {
            ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
        }
        public async Task<List<TextResult>> GetOCRResultAsync(string base64)
        {
            byte[] bytes;
            bytes = Convert.FromBase64String(base64);
            IBuffer buffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);
            InMemoryRandomAccessStream inStream = new InMemoryRandomAccessStream();
            DataWriter datawriter = new DataWriter(inStream.GetOutputStreamAt(0));
            datawriter.WriteBuffer(buffer, 0, buffer.Length);
            await datawriter.StoreAsync();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(inStream);
            SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            OcrResult ocrResult = await ocrEngine.RecognizeAsync(bitmap);
            return ConvertedResult(ocrResult);
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
