using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Dtos.Author;
using AutoMapper;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            if (_context.Authors == null)
            {
                _logger.LogWarning($"No records found for {nameof(GetAuthors)}");
                return NotFound();
            }

            try
            {
                var authors = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(await _context.Authors.ToListAsync());
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                _logger.LogWarning($"Record is null for {nameof(GetAuthor)}");
                return NotFound();
            }

            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Record not found for {nameof(GetAuthor)} - ID {id}");
                    return NotFound();
                }

                var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                _logger.LogWarning($"Record ID is invalid for {nameof(PutAuthor)} - ID {id}");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"Record {nameof(Author)} not found for {nameof(PutAuthor)} - ID {id}");
                return NotFound();
            }

            _mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    _logger.LogWarning($"Record ID does not exist for {nameof(PutAuthor)} - ID {id}");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            if (_context.Authors == null)
            {
                _logger.LogWarning("Entity set 'BookStoreDbContext.Authors' is null.");
                return Problem("Entity set 'BookStoreDbContext.Authors' is null.");
            }

            try
            {
                var author = _mapper.Map<Author>(authorDto);

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing POST in {nameof(PostAuthor)}", authorDto);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                _logger.LogWarning($"No record found for for {nameof(DeleteAuthor)} - ID {id}");
                return NotFound();
            }

            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Record {nameof(Author)} not found for {nameof(DeleteAuthor)} - ID {id}");
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing DELETE for {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
