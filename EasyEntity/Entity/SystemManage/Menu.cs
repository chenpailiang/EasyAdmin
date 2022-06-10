global using static EasyCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasyEntity.Entity;

public class Menu : AdminEntity
{
    public int id { get; init; }
    public int parentId { get; private set; }
    public string name { get; private set; }
    public string symbol { get; private set; }
    public string? icon { get; private set; }
    public string? path { get; private set; }
    public int sort { get; private set; }

    /// <summary>
    /// 新增
    /// </summary>
    public void Create()
    {
        if (db.Queryable<Menu>().Any(x => x.parentId == this.parentId && x.name == this.name))
            throw BadRequestExp("菜单已存在");
        this.creator = this.updator = CurrentHttpContext.currentAdminAccount;
        db.Insertable(this).ExecuteCommand();
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public void Update()
    {
        var menus = db.Queryable<Menu>().Where(x => x.id == this.id || x.parentId == this.parentId && x.name == this.name).ToList();
        if (!menus.Any(x => x.id == this.id))
            throw BadRequestExp("菜单不存在");
        if (menus.Count > 1)
            throw BadRequestExp("菜单名称已存在");
        this.updator = CurrentHttpContext.currentAdminAccount;
        db.Updateable(this).ExecuteCommand();
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void Delete()
    {
        if (!db.Queryable<Menu>().Any(x => x.id == this.id))
            throw BadRequestExp("菜单不存在");
        this.updator = CurrentHttpContext.currentAdminAccount;
        db.Deleteable(this).IsLogic().ExecuteCommand("oust", this.id);
    }
}
