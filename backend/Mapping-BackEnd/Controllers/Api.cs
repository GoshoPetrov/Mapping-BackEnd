using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Mapping_BackEnd.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mapping_BackEnd.Controllers
{
    public class Model
    {
        public double Lat { get; set; }
        public double Longtitude { get; set; }
        public string Caption { get; set; }
        public int Radius { get; set; }
    }

    public class Text
    {
        public string Message { get; set; }
    }
    public class Api : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _openAiApiKey = "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"; // Replace with your actual OpenAI API key

        public Api(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public IActionResult Map()
        {
            var db = new PensaClubContext();

            var list = new List<Dictionary<string, object>>();

            foreach (var item in db.Places)
            {
                var dict = new Dictionary<string, object>();

                dict["coordinates"] = new double?[] { item.Lat, item.Long };
                dict["radius"] = item.Radius;
                dict["message"] = item.Caption;

                list.Add(dict);
            }

            return Json(list);
        }

        public IActionResult Map2()
        {
            var dict = new Dictionary<string, object>();

            dict["coordinates"] = new double[] { 42.6361668, 23.3698051 };
            dict["radius"] = 18;
            dict["message"] = "Softuni";

            return Json(dict);
        }

        public IActionResult Save([FromBody] Model model)
        {
            var db = new PensaClubContext();
            var n = new Place();

            n.Lat = model.Lat;
            n.Long = model.Longtitude;
            n.Caption = model.Caption;
            n.Radius = 13;

            db.Places.Add(n);
            db.SaveChanges();

            // You can now use model.Var1 and model.Var2
            return Ok(new { model.Lat, model.Longtitude });
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] Text text)
        {
            if (string.IsNullOrEmpty(text.Message))
            {
                return BadRequest("Text input cannot be empty.");
            }

            // Prepare the request data
            var requestData = new
            {
                model = "gpt-3.5-turbo", // Or another model, like gpt-4 if available
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant. You know the coordinates of all the locations in the world. When the user ask a question answer with the coordiantes [latitude, longtitude, radius] suitable for openmaps. If continent is not specified, assume Europe. If country is not specified, assume Bulgaria. If city is not specified, assume Sofia."},
                new { role = "user", content = text.Message }
            }
            };

            // Serialize the request data to JSON using System.Text.Json
            var content = new StringContent(
                JsonSerializer.Serialize(requestData),
                Encoding.UTF8,
                "application/json"
            );

            // Add the Authorization header (API Key) to the HttpClient request
            _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiApiKey}");
    
            // Make the POST request to the OpenAI API
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error from OpenAI API: " + response.ReasonPhrase);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            // Deserialize the response using System.Text.Json
            var result = JsonSerializer.Deserialize<JsonElement>(responseString);

            // Extract the ChatGPT response from the API response
            var chatResponse = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            // Return the response as JSON

            var dict = new Dictionary<string, object>();
            dict["coordinates"] = ExtractCoordinates(chatResponse);
            dict["radius"] = 18;
            dict["message"] = text.Message;

            return Json(dict);
        }

        public double[] ExtractCoordinates(string input)
        {
            // Define the regex pattern to match all double values (both integers and floats)
            var pattern = @"-?\d+\.\d+|-?\d+"; // Match numbers (both decimal and integer)

            // Find all matches in the input string
            var matches = Regex.Matches(input, pattern);

            // Convert matched values to doubles and return as an array
            return matches.Cast<Match>()
                          .Select(match => double.Parse(match.Value))
                          .ToArray();


        }

    }


}
