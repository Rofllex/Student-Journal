using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Server.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
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
