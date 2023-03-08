using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
//using System.Web.Http;
//using StreamingApi.Services;

namespace StreamingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamReaderController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _env;

        public readonly string WebRootPath;
        public readonly string ContentRootPath;

        private readonly string videofolder;
        private readonly ILogger<StreamReaderController> logger;

        private readonly IList<string> fileList;
        public StreamReaderController(IConfiguration configuration, IWebHostEnvironment env, ILogger<StreamReaderController> logger)
        {

            this.configuration = configuration;
            _env = env;

            ContentRootPath = _env.ContentRootPath;
            WebRootPath = _env.WebRootPath;

            this.videofolder = configuration.GetValue<string>("VideoBasePath");
            this.logger = logger;

            var list = Directory.EnumerateFiles(videofolder, "*.webm");
            fileList= new List<string>();
            foreach (var file in list)
            {
                fileList.Add(file.Replace(this.videofolder,""));
            }
        }

        public async Task<IList<string>> GetVideoListAsync()
        {
            logger.LogInformation($"{DateTime.Now} : Reading streaming directory{fileList.Count} Record found");
            return fileList;
        }

        [HttpGet("file/{id}")]
        public IActionResult Get(string id)
        {
            string filename = $"Recording-{id}.webm";
            int duration = 60;
            int maxSequence = 5;
            logger.LogInformation($"Start Get(filename:{filename})");

            string filefullpath = Path.Combine(ContentRootPath,videofolder, filename);

            HttpContext.Response.Headers.Add("x-segment-duration", duration.ToString());
            HttpContext.Response.Headers.Add("x-segment-max", maxSequence.ToString());

            return PhysicalFile(filefullpath, "video/webm", filename, enableRangeProcessing: true);
        }

        ////[HttpHead]
        //////[Route("{resource}")]
        ////public System.Web.Http.IHttpActionResult Head(string resource)
        ////{
        ////    //  Get resource info here

        ////    var resourceType = "application/json";
        ////    var resourceLength = 1024;

        ////    return new Head(resourceType, resourceLength);
        ////}



    }

}
