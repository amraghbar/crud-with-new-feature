using FluentValidation;
using Task.Data;
using Task.DTOS;

namespace WebApplication1.Validators
{
    public class CreateProductDTOValidator : AbstractValidator<CreateProductDTO>
    {
        private readonly ApplicationDbContext _context;

        public CreateProductDTOValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MinimumLength(5).WithMessage("Product name must be at least 5 characters long.")
                .MaximumLength(30).WithMessage("Product name must not exceed 30 characters.")
                .Must(BeUniqueName).WithMessage("Product name must be unique.");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Price is required.")
                .InclusiveBetween(20, 3000).WithMessage("Price must be between 20 and 3000.");

            // تحقق من أن الوصف مطلوب ويجب أن يكون الطول لا يقل عن 10
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");
        }

        private bool BeUniqueName(string name)
        {
            return !_context.Products.Any(p => p.Name == name);
        }
    }
}
