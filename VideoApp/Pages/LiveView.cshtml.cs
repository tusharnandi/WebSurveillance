using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoApp.Pages
{
    public class LiveViewModel : PageModel
    {

        private readonly ILogger<LiveViewModel> logger;

        public string StreamServerURL { get; set; }
        public Guid Id { get; set; }
        public LiveViewModel(IConfiguration configuration, ILogger<LiveViewModel> logger)
        {
            this.logger = logger;
            StreamServerURL = configuration.GetValue<string>("StreamServerURL"); 
            //"https://localhost:7031";
            Id = Guid.NewGuid();
        }
        public void OnGet()
        {
        }
    }
}
