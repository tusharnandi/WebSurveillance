
using VideoStreamApi.Services;

namespace VideoStreamApi
{

    public class Startup
    {
        private readonly string AllowStreamServerOrigins = "AllowStreamServerOrigins";
        public IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPresentationServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(AllowStreamServerOrigins);
            app.UseAuthorization();


            app.UseEndpoints(endpoints => {

                endpoints.MapPost("/upload", async (HttpRequest request) =>
                {
                    SortedList<string, string> QueryString = new SortedList<string, string>();
                    if (request.QueryString.HasValue)
                    {
                        string[]? prams = null;
                        prams = request.QueryString.Value?.Replace("?", "").Split('&');
                        foreach (var p in prams)
                        {
                            var item = p.Split('=');
                            if (item.Length == 2)
                                QueryString.Add(item[0], item[1]);
                        }
                    }
                    string requestId = QueryString["rq"];

                    string hashValue = string.Empty;

                    int attempt = int.Parse(QueryString["attempt"]);
                    int seq = int.Parse(QueryString["seq"]);
                    bool isFirst = bool.Parse(QueryString["isfirst"]);
                    bool isLast = bool.Parse(QueryString["islast"]);

                    //Console.WriteLine($"{DateTime.Now} -seq:{seq}- Received Data with ContentLength: {request.ContentLength} ...");

                    try
                    {
                        //DateTime currentTime = DateTime.Now;
                        //String.Format("{0:MM-dd-yy-H-mm-ss}",currentTime);
                        string FileLocation = "./wwwroot/StreamingFiles/";
                        string suffix = $"{requestId}-{attempt}";

                        string tempfile = Path.Combine(FileLocation, $"tmpwebm-{suffix}.tmp");
                        string destfile = Path.Combine(FileLocation, $"R-{suffix}.webm");


                        hashValue = await StreamRequestHandler.WriteToFile(seq, request.Body, isFirst, isLast, tempfile);

                        Console.WriteLine($"{DateTime.Now} -{attempt}-seq:{seq}- Processing completed");

                        if (isLast)
                        {
                            //destfile = string.IsNullOrEmpty(rcName) ? destfile : Path.Combine(FileLocation, $"{rcName}.webm");

                            await StreamRequestHandler.SaveToFile(tempfile, destfile);

                            Console.WriteLine($"{DateTime.Now} -{attempt}-dest:{destfile}- Save completed");
                            File.Delete(tempfile);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{DateTime.Now} -seq:{seq}- Processing completed with Error: {ex.Message}");
                        Console.ResetColor();
                    }

                    //KeyValuePair<string,string> ret = new KeyValuePair<string, string>(requestId, hashValue);

                    StreamOperationResponse ret = new StreamOperationResponse() { requestId = requestId, seq = seq, hashValue = hashValue };
                    return Results.Ok(ret);
                });


                endpoints.MapControllers();
            });

        }

    }
}
