using FluentValidation;

namespace MixAz.Models.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Ad boş ola bilməz!").MaximumLength(25).WithMessage("Maksimum uzunluq 25 simvoldan artıq ola bilməz"); 
            RuleFor(x => x.UserSurname).NotEmpty().WithMessage("Soyad boş ola bilməz!").MaximumLength(25).WithMessage("Maksimum uzunluq 25 simvoldan artıq ola bilməz"); 
            RuleFor(x => x.UserNickName).NotEmpty().WithMessage("Login hissəsi boş ola bilməz").MaximumLength(25).WithMessage("Maksimum uzunluq 25 simvoldan artıq ola bilməz"); 
            RuleFor(x => x.UserMail).NotEmpty().WithMessage("Email hissəsi boş ola bilməz").EmailAddress().WithMessage("Mail adressinizi düzgün daxil edin"); 
            RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Şifrə boş ola bilməz!").MinimumLength(8).WithMessage("Minimum uzunluq 8 simvoldan az ola bilməz"); 
        }
    }
}