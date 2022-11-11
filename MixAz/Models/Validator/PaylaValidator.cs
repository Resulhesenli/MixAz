using FluentValidation;

namespace MixAz.Models.Validator
{
    public class PaylaValidator : AbstractValidator<Post>
    {
        public PaylaValidator()
        {
            RuleFor(x => x.PostName).NotEmpty().WithMessage("Xəbər başlığı boş ola bilməz").MaximumLength(137).WithMessage("Xəbər başlığın uzunluğu 137 simvoldan artıq ola bilməz");
            RuleFor(x => x.PostDescription).NotEmpty().WithMessage("Xəbər məzmunu boş ola bilməz");
        }
    }
}
