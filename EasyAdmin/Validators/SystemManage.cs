using EasyService.Request;
using FluentValidation;

namespace EasyAdmin.Validators;


#region 菜单/功能管理

/// <summary>
/// 新增菜单
/// </summary>
public class AddMenuValidator : AbstractValidator<AddMenuReq>
{
    public AddMenuValidator()
    {
        RuleFor(x => x.name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.title).NotEmpty().MaximumLength(20);
        RuleFor(x => x.icon).NotEmpty().MaximumLength(20).When(x => x.icon != null);
    }
}

/// <summary>
/// 编辑菜单
/// </summary>
public class EditMenuValidator : AbstractValidator<EditMenuReq>
{
    public EditMenuValidator(AddMenuValidator addMenuValidator)
    {
        Include(addMenuValidator);
        RuleFor(x => x.id).GreaterThan(0);
    }
}

/// <summary>
/// 新增功能
/// </summary>
public class AddFuncValidator : AbstractValidator<AddFuncReq>
{
    public AddFuncValidator()
    {
        RuleFor(x => x.menuId).GreaterThan(0);
        RuleFor(x => x.name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.symbol).NotEmpty().MaximumLength(20);
        RuleFor(x => x.authId).InclusiveBetween(100, 99999);
    }
}

/// <summary>
/// 新增功能
/// </summary>
public class EditFuncValidator : AbstractValidator<EditFuncReq>
{
    public EditFuncValidator()
    {
        RuleFor(x => x.id).GreaterThan(0);
        RuleFor(x => x.name).NotEmpty().MaximumLength(20);
        RuleFor(x => x.symbol).NotEmpty().MaximumLength(20);
        RuleFor(x => x.authId).InclusiveBetween(100, 99999);
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