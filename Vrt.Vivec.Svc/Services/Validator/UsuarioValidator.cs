using Vrt.Vivec.Svc.Data.Request;

namespace Vrt.Vivec.Svc.Services.Validator;

public class UsuarioValidator : AbstractValidator<Usuario>, IUsuarioValidator
{
    public UsuarioValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El campo Username no puede estar vacío.")
            .NotNull().WithMessage("El campo Username no puede estar NULL.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty().WithMessage("El campo Password no puede estar vacío.")
            .NotNull().WithMessage("El campo Password no puede estar NULL.");
    }
}
