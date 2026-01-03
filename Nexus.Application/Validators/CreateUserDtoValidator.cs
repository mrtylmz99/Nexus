using FluentValidation;
using Nexus.Application.DTOs.User;

namespace Nexus.Application.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı gereklidir.")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakteri geçmemelidir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(100).WithMessage("E-posta adresi 100 karakteri geçmemelidir.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad gereklidir.")
            .MaximumLength(100).WithMessage("Ad Soyad 100 karakteri geçmemelidir.");
    }
}
