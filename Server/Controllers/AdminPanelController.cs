using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Journal.Server.Database;

namespace Journal.Server.Controllers
{
    [Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)]
    public class AdminPanelController : Controller
    {
        public AdminPanelController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }

        private readonly JournalDbContext _dbContext;
    }
}
