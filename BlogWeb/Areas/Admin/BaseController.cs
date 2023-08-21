﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWeb.Areas.Admin
{
    [Authorize(Roles ="Admin,Writer")]
    public class BaseController : PageModel
    {
        public void Index()
        {
            
        }
    }
}
