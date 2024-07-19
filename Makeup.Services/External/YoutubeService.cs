using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using static Makeup.Services.External.YoutubeProductDto;
using Microsoft.Extensions.Configuration;

namespace Makeup.Services.External
{
    public class YoutubeService
    {
        private readonly string? _apiKey;
        private readonly HttpClient _httpClient;

        public YoutubeService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("Youtube");
            _apiKey = configuration["APIS:Youtube:Key"];

        }

        public async Task<YoutubeProductDto> GetVideosAsync(string productName)
        {
            // Construct the query to search for videos related to the product name
            var query = $"search?part=snippet&q={Uri.EscapeDataString(productName)}&type=video&maxResults=5&key={_apiKey}";

            // Send the request to the YouTube API
            var response = await _httpClient.GetAsync(query);
            response.EnsureSuccessStatusCode();

            // Read the response content
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response
            var data = JsonConvert.DeserializeObject<YoutubeSearchResponseDto>(json);

            // Extract video information from the response
            var videos = data?.Items?.Select(item => new YoutubeVideoDto
            {
                VideoId = item.Id?.VideoId,
                Title = item.Snippet?.Title,
                Description = item.Snippet?.Description,
                ThumbnailUrl = item.Snippet?.Thumbnails?.Default?.Url
            }).ToList();

            // Create a YoutubeProductDto instance and populate the Videos property
            var product = new YoutubeProductDto
            {
                Videos = videos
            };

            return product;
        }
    }
}
