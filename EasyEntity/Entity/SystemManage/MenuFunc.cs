using EasyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

public class MenuFunc : AdminEntity
{
    public int id { get; init; }
    public int menuId { get; private set; }
    public string name { get; private set; }
    public string description { get; private set; }
    public string symbol { get; private set; }
    public int authId { get; private set; }

    /// <summary>
    /// 新增
    /// </summary>
    public void Create()
    {
        if (db.Queryable<MenuFunc>().Any(x => x.menuId == this.menuId && x.symbol == this.symbol))
            throw BadRequestExp("功能已存在");
        this.creator = this.updator = CurrentHttpContext.currentAdminAccount;
        db.Insertable(this).ExecuteCommand();
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public void Update()
    {
        var menus = db.Queryable<MenuFunc>().Where(x => x.id == this.id || x.menuId == this.menuId && x.symbol == this.symbol).ToList();
        if (!menus.Any(x => x.id == this.id))
            throw BadRequestExp("功能不存在");
        if (menus.Count > 1)
            throw BadRequestExp("功能已存在");
        this.updator = CurrentHttpContext.currentAdminAccount;
        db.Updateable(this).ExecuteCommand();
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void Delete()
    {
        if (!db.Queryable<MenuFunc>().Any(x => x.id == this.id))
            throw BadRequestExp("功能不存在");
        this.updator = CurrentHttpContext.currentAdminAccount;
        db.Deleteable(this).IsLogic().ExecuteCommand("oust", this.id);
    }
}