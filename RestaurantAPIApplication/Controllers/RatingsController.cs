using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPIApplication.Models;

namespace RestaurantAPIApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public RatingsController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/Ratings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings([FromQuery] Parameters clientParameters)
        {
            if (_context.Ratings == null)
            {
                return NotFound();
            }
            var ratings = await _context.Ratings
                    .OrderBy(on => on.Id)
                    .Skip((clientParameters.PageNumber - 1) * clientParameters.PageSize)
                    .Take(clientParameters.PageSize)
                    .ToListAsync();

            var res = new List<Object>();
            res.Add(ratings);

            var link = Environment.GetEnvironmentVariable("applicationUrl").Split(";")[0];
            var nextLink = new
            {
                nextLink = link +
                "/api/Ratings?PageNumber=" +
                (clientParameters.PageNumber + 1) +
                "&PageSize=" +
                clientParameters.PageSize
            };

            res.Add(nextLink);

            return Ok(res);
        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            if (_context.Ratings == null)
            {
                return NotFound();
            }
            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return NotFound();
            }

            return rating;
        }

        // PUT: api/Ratings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, Rating rating)
        {
            if (id != rating.Id)
            {
                return BadRequest();
            }

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
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

        // POST: api/Ratings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            if (_context.Ratings == null)
            {
                return Problem("Entity set 'RestaurantDbContext.Ratings'  is null.");
            }
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            if (_context.Ratings == null)
            {
                return NotFound();
            }
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RatingExists(int id)
        {
            return (_context.Ratings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
