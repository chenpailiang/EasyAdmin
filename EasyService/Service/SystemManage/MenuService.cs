using AutoMapper;
using EasyCommon;
using EasyEntity;
using EasyEntity.Entity;
using EasyService.Request;
using EasyService.Response;

namespace EasyService.Service;

public class MenuService
{
    private readonly IMapper mapper;
    private readonly CurrentUser currentUser;
    public MenuService(IMapper mapper, CurrentUser currentUser)
    {
        this.mapper = mapper;
        this.currentUser = currentUser;
    }

    /// <summary>
    /// 获取用户具有权限的菜单
    /// </summary>
    /// <returns></returns>
    public MenuRsp GetRoleMenus()
    {
        MenuRsp rsp = new();
        if (currentUser.isSuper)
            return GetMenus();
        var roleIds = DbContext.Db.Queryable<Admin>().First(x => x.id == currentUser.id).roles;
        if (roleIds == null) return rsp;
        var roles = DbContext.Db.Queryable<Role>().Where(x => roleIds.Contains(x.id)).ToList();
        var menuIds = roles.SelectMany(x => x.menus).Distinct().ToList();
        var menus = DbContext.Db.Queryable<Menu>().Where(x => menuIds.Contains(x.id)).OrderBy(x => x.sort).ToList();
        rsp.menus = mapper.Map<List<MenuDto>>(menus);
        var funcIds = roles.SelectMany(x => x.funcs).Distinct().ToList();
        var funcs = DbContext.Db.Queryable<MenuFunc>().Where(x => funcIds.Contains(x.id)).ToList();
        rsp.funcs = mapper.Map<List<FuncDto>>(funcs);
        return rsp;
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    public MenuRsp GetMenus(string? name = null)
    {
       MenuRsp rsp = new();
        var menus = DbContext.Db.Queryable<Menu>().WhereIF(!name.Nil(), x => x.name.Contains(name!)).ToList();
        rsp.menus = mapper.Map<List<MenuDto>>(menus);
        var funcs = DbContext.Db.Queryable<MenuFunc>().ToList();
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
        menu.Create(currentUser);
    }

    /// <summary>
    /// 编辑菜单
    /// </summary>
    /// <param name="req"></param>
    public void EditMenu(EditMenuReq req)
    {
        var menu = mapper.Map<Menu>(req);
        menu.Update(currentUser);
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id"></param>
    public void DelMenu(int id)
    {
        var menu = new Menu { id = id };
        menu.Delete(currentUser);
    }

    /// <summary>
    /// 新增功能
    /// </summary>
    /// <param name="req"></param>
    public void AddFunc(AddFuncReq req)
    {
        var func = mapper.Map<MenuFunc>(req);
        func.Create(currentUser);
    }

    /// <summary>
    /// 编辑功能
    /// </summary>
    /// <param name="req"></param>
    public void EditFunc(EditFuncReq req)
    {
        var func = mapper.Map<MenuFunc>(req);
        func.Update(currentUser);
    }

    /// <summary>
    /// 删除功能
    /// </summary>
    /// <param name="id"></param>
    public void DelFunc(int id)
    {
        var func = new MenuFunc { id = id };
        func.Delete(currentUser);
    }
}
