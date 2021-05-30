using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TesteIanManipulae.Models;

namespace TesteIanManipulae.Services
{
    public class YoutubeService
    {
        private readonly string _apiUrl;
        private readonly ApiDbContext _context;
        private readonly IConfiguration _configuration;

        public YoutubeService(ApiDbContext context, IConfiguration configuration)
        {

            this._context = context;
            this._configuration = configuration;
            var apiKey = configuration.GetSection("YoutubeApiKey").Value;
            string api = string.Format("https://www.googleapis.com/youtube/v3/search?key={0}&type=video&regionCode=BR&publishedAfter=2020-01-01T00:00:00Z&publishedBefore=2020-12-31T23:59:59Z&q=manipulacao&part=snippet&maxResults=50", apiKey);
            this._apiUrl = api;
        }

        public void ImportYoutubeVideosToDatabase()
        {
            var videos = this.GetVideosFromYoutube();

            foreach (var item in videos)
            {
                if (_context.Videos.Any(x => x.YoutubeVideoId == item.YoutubeVideoId))
                    continue;

                _context.Videos.Add(item);
                 
                _context.SaveChanges();
            }
        }

        private List<Video> GetVideosFromYoutube()
        { 

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);

            HttpResponseMessage response = new HttpClient().Send(request);

            List<Video> videoList = new List<Video>();

            JArray youtubeVideoJson = (JArray)JObject.Parse(response.Content.ReadAsStringAsync().Result)["items"];

            foreach (var videoJson in youtubeVideoJson)
            {
                videoList.Add(new Video()
                {
                    YoutubeVideoId = videoJson["id"]["videoId"].ToString(),
                    Titulo = videoJson["snippet"]["title"].ToString(),
                    Autor = videoJson["snippet"]["channelTitle"].ToString(),
                    Canal = videoJson["snippet"]["channelId"].ToString(),
                    DataCriacao = Convert.ToDateTime(videoJson["snippet"]["publishedAt"]),
                    Descricao = videoJson["snippet"]["description"].ToString()
                });
            }

            return videoList;
        }
    }
}
