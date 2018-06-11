using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CognitiveApp.Models;
using Newtonsoft.Json;

namespace CognitiveApp.ApiServices {
    public class GeneralImageApiService {

        private const string UriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/analyze";

        /// <summary>
        /// Gets the analysis of the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file to analyze.</param>
        public async Task<PictureApiResult> MakeAnalysisRequestAsync(string imageFilePath, CancellationToken cancellationToken) {
            try {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.ComputerVisionApiKey);

                // Request parameters. A third optional parameter is "details".
                const string requestParameters = "visualFeatures=Categories,Description,Color";

                // Assemble the URI for the REST API Call.
                const string uri = UriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using(ByteArrayContent content = new ByteArrayContent(byteData)) {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call
                    response = await client.PostAsync(uri, content, cancellationToken);
                }

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n");
                Console.WriteLine(contentString);

                return JsonConvert.DeserializeObject<PictureApiResult>(contentString);
            } catch(Exception e) {
                Console.WriteLine("\n" + e.Message);
            }

            return null;
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        private static byte[] GetImageAsByteArray(string imageFilePath) {
            using(FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read)) {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
