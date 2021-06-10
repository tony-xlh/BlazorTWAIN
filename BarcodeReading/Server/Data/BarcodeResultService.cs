using Dynamsoft.DBR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorTWAIN_Server.Data
{
    public class BarcodeResultService
    {
        public List<TextResult> GetBarcodeResult(string base64)
        {
            BarcodeReader reader = new BarcodeReader("t0069fQAAAAM656kb3qDzrc9Yf4UYDqMekf2RajobBUxz7AB6+lUHifyq/jKD47/HS94EHH6F2prBZqFlU0W4rq4r1feL3j92");
            Dynamsoft.TextResult[] results = reader.DecodeBase64String(base64, "");
            return ConvertedResult(results);
        }

        private List<TextResult> ConvertedResult(Dynamsoft.TextResult[] results)
        {
            List<TextResult> newResults = new List<TextResult>();
            foreach (Dynamsoft.TextResult result in results)
            {
                TextResult newResult = new TextResult();
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
