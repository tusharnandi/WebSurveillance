using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration configuration;
        public Guid Id { get; set; }
        public string StreamingServerURL { get; private set; }

        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            Id = Guid.NewGuid();
            StreamingServerURL = configuration.GetValue<string>("StreamingServerURL");


        }
        public void OnGet()
        {

        }
    }
}