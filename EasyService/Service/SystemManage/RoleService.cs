global using static EasyCommon.Utility;
using AutoMapper;
using EasyEntity.Entity;
using EasyService.Response;
using EasyService.Request;
using EasyCommon;
using EasyEntity;

namespace EasyService.Service;

public class RoleService
{
    private readonly IMapper imapper;
    private readonly CurrentUser currentUser;
    public RoleService(IMapper imapper, CurrentUser currentUser)
    {
        this.imapper = imapper;
        this.currentUser = currentUser;
    }


    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    public List<RoleDto> Get()
    {
        var admins = DbContext.Db.Queryable<Role>().ToList();
        return imapper.Map<List<RoleDto>>(admins);
    }

    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="req"></param>
    public void AddRole(AddRoleReq req)
    {
        var role = imapper.Map<Role>(req);
        role.Create(currentUser);
    }

    /// <summary>
    /// 编辑角色
    /// </summary>
    /// <param name="req"></param>
    public void EditRole(EditRoleReq req)
    {
        var role = imapper.Map<Role>(req);
        role.Update(currentUser);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id"></param>
    public void DelRole(long id)
    {
        var role = new Role { id = id };
        role.Delete(currentUser);
    }

    /// <summary>
    /// 获取角色的菜单功能权限
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RoleAuthDto GetAuth(long id)
    {
        RoleAuthDto rsp = new();
        var menus = DbContext.Db.Queryable<Menu>().ToList();
        rsp.menus = imapper.Map<List<MenuDto>>(menus);
        var funcs = DbContext.Db.Queryable<MenuFunc>().ToList();
        rsp.funcs = imapper.Map<List<RoleAuthDto.RoleFunc>>(funcs);
        var role = DbContext.Db.Queryable<Role>().First(x => x.id == id);
        foreach (var item in rsp.funcs)
        {
            if (role.funcs.Any(x => x == item.id))
            {
                item.HasAuth = true;
            }
        }
        return rsp;
    }

    /// <summary>
    /// 分配权限
    /// </summary>
    public void AssignAuth(long roleId, AssignRoleReq req)
    {
        var role = new Role { id = roleId };
        role.AssignAuth(currentUser, req.menus, req.funcs);
    }

    /// <summary>
    /// 用户分配角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="req"></param>
    public void SetToAdmin(long roleId, RoleSetToAdmin req)
    {
        var role = new Role { id = roleId };
        role.SetToAdmin(currentUser, req.adminIds);
    }
}
