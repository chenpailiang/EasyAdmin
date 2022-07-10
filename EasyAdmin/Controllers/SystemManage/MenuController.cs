using EasyAdmin.Utilitys;
using EasyCommon;
using EasyService.Request;
using EasyService.Response;
using EasyService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyAdmin.Controllers;

/// <summary>
/// 菜单管理
/// </summary>
public class MenuController : BaseController
{
    #region 构造
    private readonly MenuService menuService;
    public MenuController(MenuService menuService)
    {
        this.menuService = menuService;
    }
    #endregion

    /// <summary>
    /// 获取用户具有权限的菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("own")]
    public ActionResult<MenuRsp> GetRoleMenus()
    {
        return menuService.GetRoleMenus();
    }

    /// <summary>
    /// 查询菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("{name?}"), AuthSet(AuthEnum.at120)]
    public ActionResult<MenuRsp> GetMenus(string? name)
    {
        return Ok(menuService.GetMenus(name));
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route(""), AuthSet(AuthEnum.at121)]
    public ActionResult AddMenu([FromBody] AddMenuReq req)
    {
        menuService.AddMenu(req);
        return Ok();
    }

    /// <summary>
    /// 编辑菜单
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut, Route(""), AuthSet(AuthEnum.at122)]
    public ActionResult EditMenu([FromBody] EditMenuReq req)
    {
        menuService.EditMenu(req);
        return Ok();
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单Id</param>
    /// <returns></returns>
    [HttpDelete, Route("{id}"), AuthSet(AuthEnum.at123)]
    public ActionResult DelMenu(int id)
    {
        menuService.DelMenu(id);
        return Ok();
    }

    /// <summary>
    /// 获取功能权限值
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("func/at"), AuthSet(AuthEnum.at131)]
    public ActionResult<int[]> GetFuncAuth() => Ok(Utility.EnumToNumberList(typeof(AuthEnum)));

    /// <summary>
    /// 新增功能
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost, Route("func"), AuthSet(AuthEnum.at131)]
    public ActionResult AddFunc([FromBody] AddFuncReq req)
    {
        menuService.AddFunc(req);
        return Ok();
    }

    /// <summary>
    /// 编辑功能
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut, Route("func"), AuthSet(AuthEnum.at132)]
    public ActionResult EditFunc([FromBody] EditFuncReq req)
    {
        menuService.EditFunc(req);
        return Ok();
    }

    /// <summary>
    /// 删除功能
    /// </summary>
    /// <param name="id">功能Id</param>
    /// <returns></returns>
    [HttpDelete, Route("func/{id}"), AuthSet(AuthEnum.at133)]
    public ActionResult DelFunc(int id)
    {
        menuService.DelFunc(id);
        return Ok();
    }
}
