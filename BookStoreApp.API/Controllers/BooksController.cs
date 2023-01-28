using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Dtos.Book;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            if (_context.Books == null)
            {
                _logger.LogWarning($"No records found for {nameof(GetBooks)}");
                return NotFound();
            }

            try
            {
                var books = await _context.Books.Include(a => a.Author).ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider).ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetBooks)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            if (_context.Books == null)
            {
                _logger.LogWarning($"Record is null for {nameof(GetBook)}");
                return NotFound();
            }

            try
            {
                var book = await _context.Books.Include(q => q.Author).ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(q => q.Id == id);

                if (book == null)
                {
                    _logger.LogWarning($"Record not found for {nameof(GetBook)} - ID {id}");
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                _logger.LogWarning($"Record ID is invalid for {nameof(PutBook)} - ID {id}");
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                _logger.LogWarning($"Record {nameof(Book)} not found for {nameof(PutBook)} - ID {id}");
                return NotFound();
            }

            _mapper.Map(bookDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    _logger.LogWarning($"Record ID does not exist for {nameof(PutBook)} - ID {id}");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            if (_context.Books == null)
            {
                _logger.LogWarning("Entity set 'BookStoreDbContext.Books' is null.");
                return Problem("Entity set 'BookStoreDbContext.Books' is null.");
            }

            try
            {
                var book = _mapper.Map<Book>(bookDto);

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing POST in {nameof(PostBook)}", bookDto);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                _logger.LogWarning($"No record found for for {nameof(DeleteBook)} - ID {id}");
                return NotFound();
            }

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning($"Record {nameof(Author)} not found for {nameof(DeleteBook)} - ID {id}");
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing DELETE for {nameof(DeleteBook)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
