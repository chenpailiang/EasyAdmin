using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyAdmin.Utilitys;
using EasyService.Service;
using EasyService.Request;
using EasyService.Response;
using EasyCommon;

namespace EasyAdmin.Controllers;

/// <summary>
/// 用户管理
/// </summary>
public class AdminController : BaseController
{
    #region 构造
    private readonly ILogger<AdminController> _logger;
    private readonly AdminService adminService;
    private readonly CurrentUser currentUser;

    public AdminController(ILogger<AdminController> logger, AdminService adminService, CurrentUser currentUser)
    {
        _logger = logger;
        this.adminService = adminService;
        this.currentUser = currentUser;
    }
    #endregion

    /// <summary>
    /// 获取当前管理员信息
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("own")]
    public ActionResult<AdminDto> Get()
    {
        return adminService.Get(currentUser.id);
    }

    /// <summary>
    /// 获取所有管理员信息
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route(""), AuthSet(AuthEnum.at100)]
    public ActionResult<List<AdminDto>> GetAll()
    {
        return adminService.GetAll();
    }

    /// <summary>
    /// 新增管理员
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
    /// 编辑管理员
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
    /// 删除管理员
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