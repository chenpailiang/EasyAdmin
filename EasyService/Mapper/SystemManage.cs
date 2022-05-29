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
        #region 用户管理

        CreateMap<AddAdminReq, Admin>();

        CreateMap<Admin, AdminDto>();



        #endregion

        #region 菜单管理

        #endregion

        #region 角色管理

        #endregion
    }
}
