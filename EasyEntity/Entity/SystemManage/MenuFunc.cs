using EasyCommon;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

[SugarTable("MenuFunc")]
public class MenuFunc : BaseEntity
{
    public long menuId { get; private set; }
    public string name { get; private set; }
    public string description { get; private set; }
    public string symbol { get; private set; }
    public int authId { get; private set; }

    /// <summary>
    /// 新增
    /// </summary>
    public void Create(CurrentUser currentUser)
    {
        if (db.Queryable<MenuFunc>().Any(x => x.menuId == this.menuId && x.symbol == this.symbol))
            throw BadRequest("功能已存在");
        this.creator = currentUser.account;
        db.Insertable(this).ExecuteInsert();
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public void Update(CurrentUser currentUser)
    {
        var menus = db.Queryable<MenuFunc>().Where(x => x.id == this.id || x.menuId == this.menuId && x.symbol == this.symbol).ToList();
        if (!menus.Any(x => x.id == this.id))
            throw NotFound("功能不存在");
        if (menus.Count > 1)
            throw BadRequest("功能名称已存在");
        
        db.Updateable(this).IgnoreColumns(x => x.menuId).ExecuteUpdate(currentUser);
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void Delete(CurrentUser currentUser)
    {
        if (!db.Queryable<MenuFunc>().Any(x => x.id == this.id))
            throw NotFound("功能不存在");
        db.Updateable(this).ExecuteDelete(currentUser);
    }
}