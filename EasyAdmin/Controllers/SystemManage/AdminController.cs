using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyAdmin.Utilitys;
using EasyService.Service;
using EasyService.Request;
using EasyService.Response;

namespace EasyAdmin.Controllers;

/// <summary>
/// 用户管理
/// </summary>
public class AdminController : BaseController
{
    #region 构造
    private readonly ILogger<AdminController> _logger;
    private readonly AdminService adminService;

    public AdminController(ILogger<AdminController> logger, AdminService adminService)
    {
        _logger = logger;
        this.adminService = adminService;
    }
    #endregion

    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("")]
    public ActionResult<List<AdminDto>> Get()
    {
        return default;

    }

    /// <summary>
    /// 获取指定用户
    /// </summary>
    /// <param name="id">用户id</param>
    /// <returns></returns>
    [HttpGet, Route("{id}"), AuthSet()]
    public ActionResult<AdminDto> Get(int id)
    {
        return adminService.Get(id);
    }

    /// <summary>
    /// 新增管理员
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