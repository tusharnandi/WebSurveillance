using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoApp.Pages
{
    public class RecoderModel : PageModel
    {
        private readonly ILogger<RecoderModel> logger;

        public string StreamServerURL { get; set; }
        public Guid Id { get; set; }
        public RecoderModel(IConfiguration configuration,ILogger<RecoderModel> logger)
        {
            this.logger = logger;
            StreamServerURL = configuration.GetValue<string>("StreamServerURL"); //"https://localhost:7031";
            Id = Guid.NewGuid();

        }
        public void OnGet()
        {
        }
    }
}
