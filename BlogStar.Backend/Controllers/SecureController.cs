using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Controllers
{
    using BlogStar.Backend.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {

        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
                // Доступ разрешен для аутентифицированных пользователей
                return Ok(new { Message = "Доступ разрешен" });
           
                // Доступ запрещен для неаутентифицированных пользователей
             
        }
    }
}
