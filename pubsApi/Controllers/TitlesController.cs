using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PubsApi.Models;
using PubsApi.DTOs;


namespace PubsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private readonly PubsContext _context;

        public TitlesController(PubsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _context.Titles
                .Include(b => b.TitleAuthors)
                .ThenInclude(ta => ta.Author)
                .Select(b => new BookDTO
                {
                    Title_id = b.title_id,
                    Title = b.title,
                    Type = b.type,
                    Pub_id = b.pub_id,
                    Price = b.price,
                    Advance = b.advance,
                    Royalty = b.royalty,
                    Ytd_sales = b.ytd_sales,
                    Notes = b.notes,
                    Pubdate = b.pubdate,
                    Authors = b.TitleAuthors.Select(ta => new AuthorDetailDTO
                    {
                        AuthorName = ta.Author.Au_fname + " " + ta.Author.Au_lname,
                        Au_ord = ta.Au_ord,
                        Royaltyper = ta.Royaltyper
                    }).ToList()
                })
                .OrderBy(b => b.Title_id)
                .ToListAsync();

            return Ok(books);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(string id)
        {
            var book = await _context.Titles
                .Include(b => b.TitleAuthors)
                .ThenInclude(ta => ta.Author)
                .Where(b => b.title_id == id)
                .Select(b => new BookDTO
                {
                    Title_id = b.title_id,
                    Title = b.title,
                    Type = b.type,
                    Pub_id = b.pub_id,
                    Price = b.price,
                    Advance = b.advance,
                    Royalty = b.royalty,
                    Ytd_sales = b.ytd_sales,
                    Notes = b.notes,
                    Pubdate = b.pubdate,
                    Authors = b.TitleAuthors.Select(ta => new AuthorDetailDTO
                    {
                        AuthorName = ta.Author.Au_fname + " " + ta.Author.Au_lname,
                        Au_ord = ta.Au_ord,
                        Royaltyper = ta.Royaltyper
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/Titles
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookCreateDto)
        {
            var book = new Title
            {
                title_id = bookCreateDto.Title_id,
                title = bookCreateDto.Title,
                type = bookCreateDto.Type,
                pub_id = bookCreateDto.Pub_id,
                price = bookCreateDto.Price,
                advance = bookCreateDto.Advance,
                royalty = bookCreateDto.Royalty,
                ytd_sales = bookCreateDto.Ytd_sales,
                notes = bookCreateDto.Notes,
                pubdate = bookCreateDto.Pubdate
            };

            _context.Titles.Add(book);

            // Adding the authors associated with this book
            if (bookCreateDto.Authors != null && bookCreateDto.Authors.Any())
            {
                foreach (var authorDto in bookCreateDto.Authors)
                {
                    var titleAuthor = new TitleAuthor
                    {
                        Au_id = authorDto.Au_id,
                        Title_id = book.title_id,
                        Au_ord = authorDto.Au_ord,
                        Royaltyper = authorDto.Royaltyper
                    };
                    _context.TitleAuthors.Add(titleAuthor);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookById", new { id = book.title_id }, bookCreateDto);
        }

        // PUT: api/Titles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(string id, BookCreateDto bookCreateDto)
        {
            if (id != bookCreateDto.Title_id)
            {
                return BadRequest();
            }

            var book = await _context.Titles.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.title = bookCreateDto.Title;
            book.type = bookCreateDto.Type;
            book.pub_id = bookCreateDto.Pub_id;
            book.price = bookCreateDto.Price;
            book.advance = bookCreateDto.Advance;
            book.royalty = bookCreateDto.Royalty;
            book.ytd_sales = bookCreateDto.Ytd_sales;
            book.notes = bookCreateDto.Notes;
            book.pubdate = bookCreateDto.Pubdate;

            _context.Entry(book).State = EntityState.Modified;

            // Updating the authors associated with this book
            var existingTitleAuthors = _context.TitleAuthors.Where(ta => ta.Title_id == id).ToList();
            _context.TitleAuthors.RemoveRange(existingTitleAuthors);

            if (bookCreateDto.Authors != null && bookCreateDto.Authors.Any())
            {
                foreach (var authorDto in bookCreateDto.Authors)
                {
                    var titleAuthor = new TitleAuthor
                    {
                        Au_id = authorDto.Au_id,
                        Title_id = book.title_id,
                        Au_ord = authorDto.Au_ord,
                        Royaltyper = authorDto.Royaltyper
                    };
                    _context.TitleAuthors.Add(titleAuthor);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // DELETE: api/Books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var book = await _context.Titles.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var titleAuthors = _context.TitleAuthors.Where(ta => ta.Title_id == id);
            _context.TitleAuthors.RemoveRange(titleAuthors);

            var sales = _context.Sales.Where(s => s.Title_id == id);
            _context.Sales.RemoveRange(sales);

            _context.Titles.Remove(book);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(string id)
        {
            return _context.Titles.Any(e => e.title_id == id);
        }
    }
}
