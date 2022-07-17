using EasyAdmin.Utilitys;
using EasyCommon;
using EasyService.Request;
using EasyService.Response;
using EasyService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyAdmin.Controllers;

/// <summary>
/// 角色管理
/// </summary>
public class RoleController : BaseController
{
    #region 构造
    private readonly ILogger<RoleController> _logger;
    private readonly RoleService roleService;
    private readonly CurrentUser currentUser;

    public RoleController(ILogger<RoleController> logger, RoleService roleService, CurrentUser currentUser)
    {
        _logger = logger;
        this.roleService = roleService;
        this.currentUser = currentUser;
    }
    #endregion

    /// <summary>
    /// 查询角色列表
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("{name?}"), AuthSet(AuthEnum.at110)]
    public ActionResult<List<RoleDto>> Get()
    {
        return roleService.Get();
    }

    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route(""), AuthSet(AuthEnum.at111)]
    public ActionResult AddRole(AddRoleReq req)
    {
        roleService.AddRole(req);
        return Ok();
    }

    /// <summary>
    /// 编辑角色
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut, Route(""), AuthSet(AuthEnum.at112)]
    public ActionResult EditRole(EditRoleReq req)
    {
        roleService.EditRole(req);
        return Ok();
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete, Route(""), AuthSet(AuthEnum.at113)]
    public ActionResult DelRole(long id)
    {
        roleService.DelRole(id);
        return Ok();
    }


    /// <summary>
    /// 获取角色具有的菜单功能权限
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route("{id}/auth"), AuthSet()]
    public ActionResult<RoleAuthDto> GetAuth(long id)
    {
        var rsp = roleService.GetAuth(id);
        return Ok(rsp);
    }

    /// <summary>
    /// 分配权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route("{roleId}/auth"), AuthSet(AuthEnum.at114)]
    public ActionResult AssignAuth(long roleId, AssignRoleReq req)
    {
        roleService.AssignAuth(roleId, req);
        return Ok();
    }

    /// <summary>
    /// 给用户分配角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route("{role}/toAdmin"), AuthSet(AuthEnum.at115)]
    public ActionResult SetToAdmin(long roleId, RoleSetToAdmin req)
    {
        roleService.SetToAdmin(roleId, req);
        return Ok();
    }
}
