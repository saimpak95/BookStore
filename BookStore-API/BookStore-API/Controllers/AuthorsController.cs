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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;
        private readonly ILoggerService loggerService;
        private readonly IMapper mapper;

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService loggerService, IMapper mapper)
        {
            this.authorRepository = authorRepository;
            this.loggerService = loggerService;
            this.mapper = mapper;
        }
        // Get all Authors
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAuthors()
       {
            try
            {
                loggerService.LogInfo("Attempted get all authors");
                var authors = await authorRepository.FindAll();
                var response = mapper.Map<IList<AuthorDTO>>(authors);
                loggerService.LogInfo("Successfully got all authors");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAuthor(int id)
        {
            try
            {
                loggerService.LogInfo("Attempted get an author");
                var authors = await authorRepository.FindByID(id);
                var response = mapper.Map<AuthorDTO>(authors);
                if (response == null)
                {
                    loggerService.LogWarning($"Authod with ID: {id} is not Found!");
                    return NotFound();
                }
                loggerService.LogInfo("Successfully got an author");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }
      
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                loggerService.LogInfo("Author submission submitted");
                if (authorDTO == null)
                {
                    loggerService.LogWarning("empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    loggerService.LogWarning("Author data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Create(author);
                if (!isSuccess)
                {

                    return InternalError("Author creation Failed");
                }
                loggerService.LogInfo("Author created successfully");
                return Created("Created", new { author });    

            }
            catch (Exception ex)
            {

                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            try
            { if (id < 1 || authorDTO == null || id!= authorDTO.Id)
                {
                    return BadRequest();
                }
                loggerService.LogInfo("Author submission submitted");
                if (authorDTO == null)
                {
                    loggerService.LogWarning("empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    loggerService.LogWarning("Author data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = mapper.Map<Author>(authorDTO);
                var isSuccess = await authorRepository.Update(author);
                if (!isSuccess)
                {

                    return InternalError("Author Updation Failed");
                }
                loggerService.LogInfo("Author Updated successfully");
                return NoContent();

            }
            catch (Exception ex)
            {

                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if(id < 1)
                {
                    return BadRequest();
                }
                var author = await authorRepository.FindByID(id);
                if (author == null)
                {
                    return NotFound();
                }
                var isSuccess =await authorRepository.Delete(author);
                if(!isSuccess)
                {
                    return InternalError("Author Deletion Failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {

                return InternalError($"{ex.Message} - {ex.InnerException}");
            }

        }
            private ObjectResult InternalError(string message)
        {
            loggerService.LogError(message);
            return StatusCode(500, "Something went wrong please contact Administrator");
        }
    }
}
