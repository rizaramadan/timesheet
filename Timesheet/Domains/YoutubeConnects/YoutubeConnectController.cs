using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Timesheet.Domains.YoutubeConnects
{
    public class YoutubeConnectController : Controller
    {
        private IConfiguration _config { get; }

        public YoutubeConnectController(IConfiguration configuration)
        {
            _config = configuration;
        }

        //private async Task Run()
        //{
        //    UserCredential credential;
            
        //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
        //        GoogleClientSecrets.Load(new MemoryStream(_config["Auth:Google:ClientSecret"])).Secrets,
        //        // This OAuth 2.0 access scope allows for full read/write access to the
        //        // authenticated user's account.
        //        new[] { YouTubeService.Scope.Youtube },
        //        "user",
        //        CancellationToken.None,
        //        new FileDataStore(this.GetType().ToString())
        //    );

        //    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = this.GetType().ToString()
        //    });

        //    var channelsListRequest = youtubeService.Channels.List("contentDetails");
        //    channelsListRequest.Mine = true;

        //    // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
        //    var channelsListResponse = await channelsListRequest.ExecuteAsync();

        //    foreach (var channel in channelsListResponse.Items)
        //    {
        //        // From the API response, extract the playlist ID that identifies the list
        //        // of videos uploaded to the authenticated user's channel.
        //        var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

        //        Console.WriteLine("Videos in list {0}", uploadsListId);

        //        var nextPageToken = "";
        //        while (nextPageToken != null)
        //        {
        //            var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
        //            playlistItemsListRequest.PlaylistId = uploadsListId;
        //            playlistItemsListRequest.MaxResults = 50;
        //            playlistItemsListRequest.PageToken = nextPageToken;

        //            // Retrieve the list of videos uploaded to the authenticated user's channel.
        //            var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

        //            foreach (var playlistItem in playlistItemsListResponse.Items)
        //            {
        //                // Print information about each video.
        //                Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);
        //            }

        //            nextPageToken = playlistItemsListResponse.NextPageToken;
        //        }
        //    }
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
