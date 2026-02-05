using ApiEcommerce.Constants;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")] // al agregar el apiVersion en la url saldra la vesion
    [ApiVersion("2.0")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    // [EnableCors(PolicyNames.AllowSpecificOrigin)] --> Agregando Cors a nivel de Controlador
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET Obtener todas las categorias
        [AllowAnonymous] // haciendo publico el endpoint
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[MapToApiVersion("2.0")]
        // [EnableCors("AllowSpecificOrigin")] --> Agregando Cors a nivel de metodos
        public IActionResult GetCategoriesOrderById()
        {
            var categories = _categoryRepository.GetCategories().OrderBy(cat => cat.Id);
            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDto>(category));
            }
            return Ok(categoriesDto);
        }

        // GET(id) Obtener una categoria por id
        [AllowAnonymous] // haciendo publico el endpoint
        [HttpGet("{id:int}", Name = "GetCategory")]
        //[ResponseCache(Duration = 10)] // agregar cache
        [ResponseCache(CacheProfileName = CacheProfiles.Default10)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategory(int id)
        {
            System.Console.WriteLine($"Categoria con el ID: {id} a las {DateTime.Now}");
            var category = _categoryRepository.GetCategory(id);
            System.Console.WriteLine($"Respuesta con el ID: {id}");
            if (category == null)
            {
                return NotFound($"La Categoría con el id {id} no existe");
            }
            var categoriesDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoriesDto);
        }

        // POST Agregar una categoria 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError("CustomError", "La Categoria ya existe");
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(createCategoryDto);
            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al guardar el regitro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        // PATCH Actulizar una categoria
        [HttpPatch("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"La Categoría con el id {id} no existe");
            }
            if (updateCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.CategoryExists(updateCategoryDto.Name))
            {
                ModelState.AddModelError("CustomError", "La Categoria ya existe");
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(updateCategoryDto);
            category.Id = id;
            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al actualizar el regitro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        // DELETE Eliminar una categoria
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"La Categoría con el id {id} no existe");
            }
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
            {
                return NotFound($"La Categoría con el id {id} no existe");
            }
            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("CustomError", $"Algo salio mal al Eliminar el regitro {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
