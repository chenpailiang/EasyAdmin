﻿using AutoMapper;
using EasyCommon;
using EasyEntity.Entity;
using EasyService.Request;
using EasyService.Response;

namespace EasyService.Service;

public class MenuService
{
    private readonly IMapper mapper;
    public MenuService(IMapper mapper)
    {
        this.mapper = mapper;
    }

    /// <summary>
    /// 获取用户具有权限的菜单
    /// </summary>
    /// <param name="adminId"></param>
    /// <returns></returns>
    public MenuRsp GetRoleMenus()
    {
        var roleIds = Admin.db.Queryable<Admin>().First(x => x.id == CurrentHttpContext.currentAdminId).roles;
        if (roleIds.Contains(Utility.SuperAdmin))
            return GetMenus();
        MenuRsp rsp = new();
        var roles = Role.db.Queryable<Role>().Where(x => roleIds.Contains(x.id)).ToList();
        var menuIds = roles.SelectMany(x => x.menus).Distinct().ToList();
        var menus = Menu.db.Queryable<Menu>().Where(x => menuIds.Contains(x.id)).OrderBy(x => x.sort).ToList();
        rsp.menus = mapper.Map<List<MenuDto>>(menus);
        var funcIds = roles.SelectMany(x => x.funcs).Distinct().ToList();
        var funcs = MenuFunc.db.Queryable<MenuFunc>().Where(x => funcIds.Contains(x.id)).ToList();
        rsp.funcs = mapper.Map<List<FuncDto>>(funcs);
        return rsp;
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    public MenuRsp GetMenus(string? title = null)
    {
        MenuRsp rsp = new();
        var menus = Menu.db.Queryable<Menu>().WhereIF(!title.Nil(), x => x.title.Contains(title!)).ToList();
        rsp.menus = mapper.Map<List<MenuDto>>(menus);
        var funcs = MenuFunc.db.Queryable<MenuFunc>().ToList();
        rsp.funcs = mapper.Map<List<FuncDto>>(funcs);
        return rsp;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="req"></param>
    public void AddMenu(AddMenuReq req)
    {
        var menu = mapper.Map<Menu>(req);
        menu.Create();
    }

    /// <summary>
    /// 编辑菜单
    /// </summary>
    /// <param name="req"></param>
    public void EditMenu(EditMenuReq req)
    {
        var menu = mapper.Map<Menu>(req);
        menu.Update();
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id"></param>
    public void DelMenu(int id)
    {
        var menu = new Menu { id = id };
        menu.Delete();
    }

    /// <summary>
    /// 新增功能
    /// </summary>
    /// <param name="req"></param>
    public void AddFunc(AddFuncReq req)
    {
        var func = mapper.Map<MenuFunc>(req);
        func.Create();
    }

    /// <summary>
    /// 编辑功能
    /// </summary>
    /// <param name="req"></param>
    public void EditFunc(EditFuncReq req)
    {
        var func = mapper.Map<MenuFunc>(req);
        func.Update();
    }

    /// <summary>
    /// 删除功能
    /// </summary>
    /// <param name="id"></param>
    public void DelFunc(int id)
    {
        var func = new MenuFunc { id = id };
        func.Delete();
    }
}
