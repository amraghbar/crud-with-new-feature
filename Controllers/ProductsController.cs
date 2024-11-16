using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Task.Data;
using Task.DTOS;
using Task.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var products = _context.Products.ToList();
            var dtoList = products.Adapt<IEnumerable<GetAllProductDTO>>();
            return Ok(dtoList);
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateProductDTO dto, [FromServices] IValidator<CreateProductDTO> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var modelState = new ModelStateDictionary();
                validationResult.Errors.ForEach(e => modelState.AddModelError(e.PropertyName, e.ErrorMessage));
                return ValidationProblem(modelState);
            }

            var product = dto.Adapt<Product>();
            _context.Products.Add(product);
            _context.SaveChanges();

            var createdDto = product.Adapt<GetAllProductDTO>();
            return Ok(createdDto);
        }

        [HttpGet("Get")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var dto = product.Adapt<GetAllProductDTO>();
            return Ok(dto);
        }

        [HttpPut("Update")]
        public IActionResult Update(int id, CreateProductDTO dto, [FromServices] IValidator<CreateProductDTO> validator)
        {
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var modelState = new ModelStateDictionary();
                validationResult.Errors.ForEach(e => modelState.AddModelError(e.PropertyName, e.ErrorMessage));
                return ValidationProblem(modelState);
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            dto.Adapt(product);
            _context.SaveChanges();

            var updatedDto = product.Adapt<GetAllProductDTO>();
            return Ok(updatedDto);
        }

        [HttpDelete("Remove")]
        public IActionResult Remove(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok("Success");
        }
    }
}
