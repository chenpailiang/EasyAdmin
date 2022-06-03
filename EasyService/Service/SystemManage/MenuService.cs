using AutoMapper;
using EasyCommon;
using EasyEntity.Entity;
using EasyService.Request;
using EasyService.Response;

namespace EasyService.Service;

public class MenuService : BaseService
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
    public RoleMenuRsp GetMenus(int adminId)
    {
        var roleIds = Admin.db.Queryable<Admin>().First(x => x.id == adminId).roles;
        RoleMenuRsp rsp = new();
        if (roleIds.Contains((int)RoleEnum.超级管理员))
        {
            var menus = Menu.db.Queryable<Menu>().ToList();
            rsp.menus = mapper.Map<List<RoleMenuRsp.MenuDto>>(menus);
            var funcs = MenuFunc.db.Queryable<MenuFunc>().ToList();
            rsp.funcs = mapper.Map<List<RoleMenuRsp.FuncDto>>(funcs);
        }
        else
        {
            var roles = Role.db.Queryable<Role>().Where(x => roleIds.Contains((int)x.role)).ToList();
            var menuIds = roles.SelectMany(x => x.menus).Distinct().ToList();
            var menus = Menu.db.Queryable<Menu>().Where(x => menuIds.Contains(x.id)).ToList();
            rsp.menus = mapper.Map<List<RoleMenuRsp.MenuDto>>(menus);
            var funcIds = roles.SelectMany(x => x.funcs).Distinct().ToList();
            var funcs = MenuFunc.db.Queryable<MenuFunc>().Where(x => funcIds.Contains(x.id)).ToList();
            rsp.funcs = mapper.Map<List<RoleMenuRsp.FuncDto>>(funcs);
        }
        return rsp;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="req"></param>
    public void AddMenu(AddMenuReq req)
    {
        if (Menu.db.Queryable<Menu>().Any(x => x.parentId == req.parentId && x.name == req.name && !x.oust)) throw BadRequestExp("菜单已存在");
        var menu = mapper.Map<Menu>(req);
        Menu.db.Insertable(menu).ExecuteCommand();
    }
}
