﻿using MagicVilla_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Villa> GetVillas() {
            return new List<Villa>
            {
                new Villa{Id=1, Name="Con vista a la pileta"},
                new Villa{Id=1, Name="Con vista a la playa"}
            };
        }
    }
}
