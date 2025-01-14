﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Data;
using UrlShortenerService.Models;

namespace UrlShortenerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly UrlShortenerServiceContext _context;

        public UrlsController(UrlShortenerServiceContext context)
        {
            _context = context;
        }

        // GET: api/Urls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Url>>> GetUrl()
        {
            return await _context.Url.ToListAsync();
        }

        // GET: api/Urls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Url>> GetUrl(int id)
        {
            var url = await _context.Url.FindAsync(id);

            if (url == null)
            {
                return NotFound();
            }

            return url;
        }

        // PUT: api/Urls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrl(int id, Url url)
        {
            if (id != url.Id)
            {
                return BadRequest();
            }

            _context.Entry(url).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrlExists(id))
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

        // KHAC
        public class UrlRequest
        {
            public string originalUrl { get; set; }
            public int? userId { get; set; }
        }

        // POST: api/Urls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] UrlRequest request)
        {
            if (string.IsNullOrEmpty(request.originalUrl))
            {
                return BadRequest("Original url cannot be null or empty.");
            }

            // Default userId to 1 if not provided
            int userId = request.userId ?? 1;

            var new_url = new Url()
            {
                OriginalUrl = request.originalUrl,
                ShortCode = GenerateShortCode(),
                CreatedAt = DateTime.Now,
                UserId = userId,
            };

            _context.Url.Add(new_url);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUrl", new { id = new_url.Id }, new_url.ShortCode);
        }

        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }

        // DELETE: api/Urls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            var url = await _context.Url.FindAsync(id);
            if (url == null)
            {
                return NotFound();
            }

            _context.Url.Remove(url);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrlExists(int id)
        {
            return _context.Url.Any(e => e.Id == id);
        }

        // KHÁC

        [HttpGet("userLinks/{userId}")]        // get all urls of the specified user
        public async Task<ActionResult<IEnumerable<Url>>> GetUserShortenedLinks(int userId)
        {
            var userLinks = await _context.Url
                                          .Where(s => s.UserId == userId)
                                          .ToListAsync();

            if (userLinks == null || !userLinks.Any())
            {
                return NotFound("No shortened links found for the specified user.");
            }

            return Ok(userLinks);
        }

        // KHAC
        [HttpGet("redirect/{shortCode}")]
        public async Task<IActionResult> GetOriginalLink(string shortCode, [FromQuery] int UserID)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return BadRequest("Short code cannot be null or empty.");
            }

            // Find the original link associated with the provided shorted link
            var shortedLink = await _context.Url.FirstOrDefaultAsync(m => m.ShortCode == shortCode && m.UserId == UserID);

            if (shortedLink == null)
            {
                return NotFound("The provided shorted link does not exist.");
            }

            // Return the original link
            return Ok(shortedLink.OriginalUrl);
        }
    }
}
