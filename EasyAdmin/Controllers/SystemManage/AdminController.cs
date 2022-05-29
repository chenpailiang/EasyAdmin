using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyAdmin.Utilitys;
using EasyService.Service;
using EasyService.Request;
using EasyService.Response;

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
    /// ��ȡ�����û�
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("")]
    public ActionResult<List<AdminDto>> Get()
    {
        return default;

    }

    /// <summary>
    /// ��ȡָ���û�
    /// </summary>
    /// <param name="id">�û�id</param>
    /// <returns></returns>
    [HttpGet, Route("{id}"), AuthSet()]
    public ActionResult<AdminDto> Get(int id)
    {
        return adminService.Get(id);
    }

    /// <summary>
    /// ��������Ա
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route(""), AuthSet(AuthEnum.at103)]
    public ActionResult AddAdmin(AddAdminReq req)
    {
        var res = adminService.AddAdmin(req);
        return Ok();
    }
}