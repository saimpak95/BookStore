using AutoMapper;
using BookStore_API.Services;
using BookStore_DomainModels;
using BookStore_DTO;
using BookStore_Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository bookRepository;
        private readonly ILoggerService loggerService;
        private readonly IMapper mapper;

        public BooksController(IBookRepository bookRepository, ILoggerService loggerService, IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.loggerService = loggerService;
            this.mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBooks()
        {
            var location = GetControllerActionName();
            try
            {
                loggerService.LogInfo($"{location}: Attempted get all Books");
                var books = await bookRepository.FindAll();
                var response = mapper.Map<IList<BookDTO>>(books);
                loggerService.LogInfo($"{location}: Successfully got all Books");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAuthor(int id)
        {
            var location = GetControllerActionName();
            try
            {
                loggerService.LogInfo("Attempted get an Book");
                var authors = await bookRepository.FindByID(id);
                var response = mapper.Map<BookDTO>(authors);
                if (response == null)
                {
                    loggerService.LogWarning($"{location}: book with ID: {id} is not Found!");
                    return NotFound();
                }
                loggerService.LogInfo("Successfully got an Book");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionName();
            try
            {
                loggerService.LogInfo($"{location}: Book Created Attempted");
                if (bookDTO == null)
                {
                    loggerService.LogWarning($"{location}: empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    loggerService.LogWarning($"{location}: Book data was Incomplete");
                    return BadRequest(ModelState);
                }
                var book = mapper.Map<Book>(bookDTO);
                var isSuccess = await bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Book creation Failed");
                }
                loggerService.LogInfo($"{location}: Book created successfully");
                return Created("Created", new { book });
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var location = GetControllerActionName();
            try
            {
                if (id < 1 || bookDTO == null || id != bookDTO.Id)
                {
                    return BadRequest();
                }
                loggerService.LogInfo($"{location}: Author submission submitted");
                if (bookDTO == null)
                {
                    loggerService.LogWarning($"{location}: empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    loggerService.LogWarning($"{location}: Author data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Book>(bookDTO);
                var isSuccess = await bookRepository.Update(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Author Updation Failed");
                }
                loggerService.LogInfo($"{location}: Author Updated successfully");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var location = GetControllerActionName();
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                var author = await bookRepository.FindByID(id);
                if (author == null)
                {
                    return NotFound();
                }
                var isSuccess = await bookRepository.Delete(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Author Deletion Failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
        }
        private string GetControllerActionName()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }
        private ObjectResult InternalError(string message)
        {
            loggerService.LogError(message);
            return StatusCode(500, "Something went wrong please contact Administrator");
        }
    }
}
