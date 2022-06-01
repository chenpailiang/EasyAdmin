using EasyService.Request;
using FluentValidation;

namespace EasyAdmin.Validators;


#region 菜单管理

public class AddMenuReqValidator : AbstractValidator<AddMenuReq>
{
    public AddMenuReqValidator()
    {
        RuleFor(x => x.name).MaximumLength(20); 
        RuleFor(x => x.title).MaximumLength(20);
        RuleFor(x => x.icon).MaximumLength(20);
        RuleFor(x => x.path).MaximumLength(50);
        RuleFor(x => x.component).MaximumLength(50);
        RuleFor(x => x.redirect).MaximumLength(50);
    }
}

#endregion

#region 用户管理

public class AddAdminReqValidator : AbstractValidator<AddAdminReq>
{
    public AddAdminReqValidator()
    {
        RuleFor(x => x.account).NotEmpty().Length(6, 10);
        RuleFor(x => x.name).NotEmpty().Length(2, 10);
    }
}
#endregion