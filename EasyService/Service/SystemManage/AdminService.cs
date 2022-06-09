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
    public AdminDto Get(int id)
    {
        var admin = Admin.Get(id);
        if (admin == null) throw BadRequestExp("用户不存在");
        return imapper.Map<AdminDto>(admin);
    }

    public AdminDto AddAdmin(AddAdminReq addAdminReq)
    {
        var admin = imapper.Map<Admin>(addAdminReq);
        return imapper.Map<AdminDto>(admin);
    }
}
