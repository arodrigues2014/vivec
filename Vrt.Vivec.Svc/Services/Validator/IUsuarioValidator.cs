using Vrt.Vivec.Svc.Data.Request;

namespace Vrt.Vivec.Svc.Services.Validator;

public interface IUsuarioValidator
{
    ValidationResult Validate(Usuario usuarioDTO);
}
