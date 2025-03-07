﻿using Common.Classes.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strings = REST_API_SERVER.Classes.CommonLocal.Strings;

namespace REST_API_SERVER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryCodesController : ControllerBase
    {
        private readonly CoffeeShopContext _context;

        public CountryCodesController(CoffeeShopContext context)
        {
            this._context = context;
        }

        // GET: api/CountryCodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryCodes>>> GetAllCountries()
        {
            return await this._context.CountryCodes.ToListAsync();
        }

        // GET: api/CountryCodes/id/5
        [HttpGet("id/{id}")]
        public async Task<ActionResult<CountryCodes>> GetCountryById(int id)
        {
            var country = await this._context.CountryCodes.FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.CountryCodesController.CountryNotFoundById
                });
            }

            return country;
        }

        // GET: api/CountryCodes/shortname/US
        [HttpGet("shortname/{shortName}")]
        public async Task<ActionResult<CountryCodes>> GetCountryByShortName(string shortName)
        {
            var country = await this._context.CountryCodes
                .FirstOrDefaultAsync(c => c.CountryTicker == shortName);

            if (country == null)
            {
                return this.NotFound(new
                {
                    Message = Strings.CountryCodesController.CountryNotFoundByTicker
                });
            }

            return country;
        }
    }
}
