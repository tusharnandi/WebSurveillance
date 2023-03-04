using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoApp.Pages
{
    public class PlayerModel : PageModel
    {

        private readonly ILogger<PlayerModel> logger;

        public string StreamServerURL { get; set; }
        public Guid Id { get; set; }
        public PlayerModel(IConfiguration configuration, ILogger<PlayerModel> logger)
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
