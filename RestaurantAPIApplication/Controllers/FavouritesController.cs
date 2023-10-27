using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPIApplication.Models;

namespace RestaurantAPIApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public FavouritesController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favourite>>> GetFavourites([FromQuery] Parameters clientParameters)
        {
          if (_context.Favourites == null)
          {
              return NotFound();
          }
            var favourites = await _context.Favourites
                      .OrderBy(on => on.Id)
                      .Skip((clientParameters.PageNumber - 1) * clientParameters.PageSize)
                      .Take(clientParameters.PageSize)
                      .ToListAsync();

            Uri uri = new(Request.GetDisplayUrl());

            var res = new
            {
                nextLink = uri.GetLeftPart(UriPartial.Authority) + "/api/Favourites?PageNumber=" + (clientParameters.PageNumber + 1) + "&PageSize=" + clientParameters.PageSize,
                data = favourites
            };

            return Ok(res);
        }

        // GET: api/Favourites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favourite>> GetFavourite(int id)
        {
          if (_context.Favourites == null)
          {
              return NotFound();
          }
            var favourite = await _context.Favourites.FindAsync(id);

            if (favourite == null)
            {
                return NotFound();
            }

            return favourite;
        }

        // PUT: api/Favourites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavourite(int id, Favourite favourite)
        {
            if (id != favourite.Id)
            {
                return BadRequest();
            }

            _context.Entry(favourite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Favourites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Favourite>> PostFavourite(Favourite favourite)
        {
          if (_context.Favourites == null)
          {
              return Problem("Entity set 'RestaurantDbContext.Favourites'  is null.");
          }
            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavourite", new { id = favourite.Id }, favourite);
        }

        // DELETE: api/Favourites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            if (_context.Favourites == null)
            {
                return NotFound();
            }
            var favourite = await _context.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavouriteExists(int id)
        {
            return (_context.Favourites?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
