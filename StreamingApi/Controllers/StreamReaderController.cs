using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string file = fileList[id];
            logger.LogInformation($"Start Get({id})");
            //string fileName = "big_buck_bunny_720p_30mb.mp4";
           // string absPath = HostingEnvironment.MapPath(virtualDirectoryPath);


            string filefullpath = Path.Combine(ContentRootPath,videofolder, file);
            return PhysicalFile(filefullpath, "video/webm", file, enableRangeProcessing: true);
        }

    }
}
