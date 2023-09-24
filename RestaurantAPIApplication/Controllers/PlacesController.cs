﻿using System;
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
    public class PlacesController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public PlacesController(RestaurantDbContext context)
        {
            _context = context;
        }

        // GET: api/Places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces([FromQuery] Parameters clientParameters)
        {
            if (_context.Places == null)
            {
                return NotFound();
            }
            var places = await _context.Places
                  .OrderBy(on => on.Id)
                  .Skip((clientParameters.PageNumber - 1) * clientParameters.PageSize)
                  .Take(clientParameters.PageSize)
                  .ToListAsync();

            var res = new List<Object>();
            res.Add(places);

            var link = Environment.GetEnvironmentVariable("applicationUrl").Split(";")[0];
            var nextLink = new
            {
                nextLink = link +
                "/api/Places?PageNumber=" +
                (clientParameters.PageNumber + 1) +
                "&PageSize=" +
                clientParameters.PageSize
            };

            res.Add(nextLink);
            return Ok(res);
        }

        // GET: api/Places/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            if (_context.Places == null)
            {
                return NotFound();
            }
            var place = await _context.Places.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            return place;
        }

        // PUT: api/Places/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlace(int id, Place place)
        {
            if (id != place.Id)
            {
                return BadRequest();
            }

            _context.Entry(place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
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

        // POST: api/Places
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Place>> PostPlace(Place place)
        {
            if (_context.Places == null)
            {
                return Problem("Entity set 'RestaurantDbContext.Places'  is null.");
            }
            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlace", new { id = place.Id }, place);
        }

        // DELETE: api/Places/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            if (_context.Places == null)
            {
                return NotFound();
            }
            var place = await _context.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaceExists(int id)
        {
            return (_context.Places?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
