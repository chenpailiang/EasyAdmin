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
    public void DelRole(int id)
    {
        var role = new Role { id = id };
        role.Delete(currentUser);
    }
}
