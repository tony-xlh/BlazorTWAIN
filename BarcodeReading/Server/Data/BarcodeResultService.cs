using Dynamsoft;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorTWAIN_Server.Data
{
    public class BarcodeResultService
    {
        public async Task<TextResult[]> GetBarcodeResultAsync(string baseuri,byte[] fileBytes, string filename)
        {
            using var content = new MultipartFormDataContent();
            content.Add(
                content: new ByteArrayContent(fileBytes),
                name: "\"file\"", // name must match the endpoint's parameter name
                fileName: filename
                );

            HttpClient Http = new HttpClient();
            Console.WriteLine(baseuri);
            var response = await Http.PostAsync(baseuri+"api/ReadBarcodes", content);

            // Was the post a success?
            response.EnsureSuccessStatusCode();

            // Where was the resource saved?        
            string jsonString = await response.Content.ReadAsStringAsync();
            TextResult[] results = new TextResult[0];
            results = System.Text.Json.JsonSerializer.Deserialize<TextResult[]>(jsonString);
            return results;
        }
    }
}
