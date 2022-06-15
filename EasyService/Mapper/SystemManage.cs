using AutoMapper;
using EasyEntity.Entity;
using EasyService.Request;
using EasyService.Response;

namespace EasyService.Mapper;

/// <summary>
/// 实体映射
/// </summary>
public class SystemManageProfile : Profile
{
    public SystemManageProfile()
    {
        this.ClearPrefixes();

        #region 用户管理

        CreateMap<AddAdminReq, Admin>();
        CreateMap<EditAdminReq, Admin>();

        CreateMap<Admin, AdminDto>();
        #endregion

        #region 菜单管理

        CreateMap<AddMenuReq, Menu>();
        CreateMap<EditMenuReq, Menu>();
        CreateMap<AddFuncReq, MenuFunc>();
        CreateMap<EditFuncReq, MenuFunc>();

        CreateMap<Menu, MenuDto>();
        CreateMap<MenuFunc, FuncDto>();
        #endregion

        #region 角色管理

        CreateMap<AddRoleReq, Role>();
        CreateMap<EditRoleReq, Role>();
        
        CreateMap<Role, RoleDto>();
        #endregion
    }
}
