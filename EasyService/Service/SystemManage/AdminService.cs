global using static EasyCommon.Utility;
using AutoMapper;
using EasyEntity.Entity;
using EasyService.Response;
using EasyService.Request;
using EasyCommon;
using EasyEntity;

namespace EasyService.Service;

public class AdminService
{
    private readonly IMapper imapper;
    private readonly CurrentUser currentUser;
    public AdminService(IMapper imapper, CurrentUser currentUser)
    {
        this.imapper = imapper;
        this.currentUser = currentUser;
    }

    /// <summary>
    /// 获取管理员信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AdminDto Get(long id)
    {
        var admin = DbContext.Db.Queryable<Admin>().IgnoreColumns(x => x.pwd).First(x => x.id == id);
        if (admin == null) throw NotFound("用户不存在");
        return imapper.Map<AdminDto>(admin);
    }

    /// <summary>
    /// 获取所有管理员信息
    /// </summary>
    /// <returns></returns>
    public List<AdminDto> GetAll()
    {
        var admins = DbContext.Db.Queryable<Admin>().IgnoreColumns(x => x.pwd).ToList();
        return imapper.Map<List<AdminDto>>(admins);
    }

    /// <summary>
    /// 新增管理员
    /// </summary>
    /// <param name="req"></param>
    public void AddAdmin(AddAdminReq req)
    {
        var admin = imapper.Map<Admin>(req);
        admin.Create(currentUser);
    }

    /// <summary>
    /// 编辑管理员
    /// </summary>
    /// <param name="req"></param>
    public void EditAdmin(EditAdminReq req)
    {
        var admin = imapper.Map<Admin>(req);
        admin.Update(currentUser);
    }

    /// <summary>
    /// 删除管理员
    /// </summary>
    /// <param name="id"></param>
    public void DelAdmin(long id)
    {
        var admin = new Admin { id = id };
        admin.Delete(currentUser);
    }

    /// <summary>
    /// 根据角色获取用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public RoleAdminDto RoleAdmin(long roleId)
    {
        RoleAdminDto rsp = new() { HasAssign = new(), Others = new() };
        var admins = DbContext.Db.Queryable<Admin>().Select(x => new { x.id, x.account, x.name, x.roles }).ToList();
        foreach (var item in admins)
        {
            if (item.roles != null && item.roles.Contains(roleId))
                rsp.HasAssign.Add(imapper.Map<AdminDto>(item));
            else
                rsp.Others.Add(imapper.Map<AdminDto>(item));
        }
        return rsp;
    }
}
