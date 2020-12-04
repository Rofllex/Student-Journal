using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;

namespace Journal.Server.Controllers
{
    [ApiController]
    [Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)]
    [Authorize]
    public class RatingController : Controller
    {
        private readonly JournalDbContext dbContext;
        public RatingController()
        {
            dbContext = JournalDbContext.CreateContext();
        }

        //public IActionResult PasteRating()
        
    }
}
