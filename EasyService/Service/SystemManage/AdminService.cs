global using static EasyCommon.Utility;
using AutoMapper;
using EasyEntity.Entity;
using EasyService.Response;
using EasyService.Request;

namespace EasyService.Service;

public class AdminService
{
    private readonly IMapper imapper;
    public AdminService(IMapper imapper)
    {
        this.imapper = imapper;
    }

    /// <summary>
    /// 获取管理员信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AdminDto Get(int id)
    {
        var admin = Admin.db.Queryable<Admin>().IgnoreColumns(x => x.pwd).First(x => x.id == id);
        if (admin == null) throw BadRequestExp("用户不存在");
        return imapper.Map<AdminDto>(admin);
    }

    /// <summary>
    /// 获取所有管理员信息
    /// </summary>
    /// <returns></returns>
    public List<AdminDto> GetAll()
    {
        var admins = Admin.db.Queryable<Admin>().IgnoreColumns(x => x.pwd).ToList();
        return imapper.Map<List<AdminDto>>(admins);
    }

    /// <summary>
    /// 新增管理员
    /// </summary>
    /// <param name="req"></param>
    public void AddAdmin(AddAdminReq req)
    {
        var admin = imapper.Map<Admin>(req);
        admin.Create();
    }

    /// <summary>
    /// 编辑管理员
    /// </summary>
    /// <param name="req"></param>
    public void EditAdmin(EditAdminReq req)
    {
        var admin = imapper.Map<Admin>(req);
        admin.Update();
    }

    /// <summary>
    /// 删除管理员
    /// </summary>
    /// <param name="id"></param>
    public void DelAdmin(int id)
    {
        var admin = new Admin { id = id };
        admin.Delete();
    }
}
