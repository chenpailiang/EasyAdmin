using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyAdmin.Utilitys;
using EasyService.Service;
using EasyService.Request;
using EasyService.Response;
using EasyCommon;

namespace EasyAdmin.Controllers;

/// <summary>
/// �û�����
/// </summary>
public class AdminController : BaseController
{
    #region ����
    private readonly ILogger<AdminController> _logger;
    private readonly AdminService adminService;

    public AdminController(ILogger<AdminController> logger, AdminService adminService)
    {
        _logger = logger;
        this.adminService = adminService;
    }
    #endregion

    /// <summary>
    /// ��ȡ��ǰ����Ա��Ϣ
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("own")]
    public ActionResult<AdminDto> Get()
    {
        return adminService.Get(CurrentHttpContext.currentAdminId);
    }

    /// <summary>
    /// ��ȡ���й���Ա��Ϣ
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route(""), AuthSet(AuthEnum.at100)]
    public ActionResult<List<AdminDto>> GetAll()
    {
        return adminService.GetAll();
    }

    /// <summary>
    /// ��������Ա
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route(""), AuthSet(AuthEnum.at101)]
    public ActionResult AddAdmin(AddAdminReq req)
    {
        adminService.AddAdmin(req);
        return Ok();
    }

    /// <summary>
    /// �༭����Ա
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut, Route(""), AuthSet(AuthEnum.at102)]
    public ActionResult EditAdmin(EditAdminReq req)
    {
        adminService.EditAdmin(req);
        return Ok();
    }

    /// <summary>
    /// ɾ������Ա
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete, Route(""), AuthSet(AuthEnum.at103)]
    public ActionResult DelAdmin(int id)
    {
        adminService.DelAdmin(id);
        return Ok();
    }
}