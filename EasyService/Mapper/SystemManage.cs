﻿using AutoMapper;
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

        CreateMap<Admin, AdminDto>();

        CreateMap<EditAdminReq, Admin>();

        #endregion

        #region 菜单管理

        CreateMap<AddMenuReq, Menu>();

        CreateMap<EditMenuReq, Menu>();

        CreateMap<Menu, MenuDto>();

        CreateMap<MenuFunc, FuncDto>();

        CreateMap<AddFuncReq, MenuFunc>();

        CreateMap<EditFuncReq, MenuFunc>();
        #endregion

        #region 角色管理

        #endregion
    }
}
