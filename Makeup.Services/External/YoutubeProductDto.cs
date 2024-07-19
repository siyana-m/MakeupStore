using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services.External
{
    public class YoutubeProductDto
    {
        public List<YoutubeVideoDto>? Videos { get; set; }
        public class YoutubeSearchResponseDto
        {
            public List<YoutubeSearchItemDto>? Items { get; set; }
        }

        public class YoutubeSearchItemDto
        {
            public YoutubeVideoIdDto? Id { get; set; }
            public YoutubeSnippetDto? Snippet { get; set; }
        }

        public class YoutubeVideoIdDto
        {
            public string? VideoId { get; set; }
        }

        public class YoutubeSnippetDto
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public YoutubeThumbnailDto? Thumbnails { get; set; }
        }

        public class YoutubeThumbnailDto
        {
            public YoutubeThumbnailDetailsDto? Default { get; set; }
        }

        public class YoutubeThumbnailDetailsDto
        {
            public string? Url { get; set; }
        }

        // Define DTO for representing YouTube video information
        public class YoutubeVideoDto
        {
            public string? VideoId { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? ThumbnailUrl { get; set; }
        }


    }
}
