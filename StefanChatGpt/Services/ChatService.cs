using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace StefanChatGpt.Services
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ChatOptions> _options;
        public ChatService(HttpClient httpClient, IOptions<ChatOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<Message> CreateChatCompletion(List<Message> messages)
        {
            var request = new { model = _options.Value.GtpModel, messages = messages.ToArray() };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.Value.ApiKey);

            var response = await _httpClient.PostAsJsonAsync(_options.Value.ApiUrl, request);
            response.EnsureSuccessStatusCode();

            var chatCompletionResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();
            return chatCompletionResponse?.choices.First().message;

        }
    }
}
