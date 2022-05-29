using EasyService.Request;
using FluentValidation;

namespace EasyAdmin.Validators;

public class AddAdminReqValidator : AbstractValidator<AddAdminReq>
{
    public AddAdminReqValidator()
    {
        RuleFor(x => x.account).NotEmpty().Length(6, 10);
        RuleFor(x => x.name).NotEmpty().Length(2, 10);
    }
}