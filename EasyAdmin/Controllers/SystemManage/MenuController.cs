using EasyAdmin.Utilitys;
using EasyCommon;
using EasyService.Request;
using EasyService.Response;
using EasyService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyAdmin.Controllers;

public class MenuController : BaseController
{
    #region 构造
    private readonly MenuService menuService;
    private readonly ScopeHttpContext httpContext;
    public MenuController(MenuService menuService, ScopeHttpContext httpContext)
    {
        this.menuService = menuService;
        this.httpContext = httpContext;
    }
    #endregion

    /// <summary>
    /// 获取用户具有权限的菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("admin")]
    public ActionResult<RoleMenuRsp> GetMenus()
    {
        return menuService.GetMenus(httpContext.currentAdminId);
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route(""), AuthSet(AuthEnum.at141)]
    public ActionResult AddMenu([FromBody] AddMenuReq req)
    {
        menuService.AddMenu(req);
        return Ok();
    }
}
