using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;

namespace CognitiveApp.ApiServices {
    public class TextApiService {
        private readonly ITextAnalyticsAPI _client = new TextAnalyticsAPI(new ApiKeyServiceClientCredentials()) {
            AzureRegion = AzureRegions.Westcentralus
        };

        /// <summary>
        /// Container for subscription credentials.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        private class ApiKeyServiceClientCredentials : ServiceClientCredentials {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
                request.Headers.Add("Ocp-Apim-Subscription-Key", Constants.TextAnalyticsApiKey);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        public async Task<SentimentBatchResult> UploadAndDetectText(string text, CancellationToken cancellationToken) {
            return await _client.SentimentAsync(new MultiLanguageBatchInput(new List<MultiLanguageInput> { new MultiLanguageInput("en", "0", text) }), cancellationToken);
        }
    }
}
