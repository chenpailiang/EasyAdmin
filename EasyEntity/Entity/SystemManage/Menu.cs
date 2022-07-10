global using static EasyCommon.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasyEntity.Entity;

public class Menu : BaseEntity
{
    public long parentId { get; private set; }
    public string name { get; private set; }
    public string symbol { get; private set; }
    public string? icon { get; private set; }
    public int sort { get; private set; }

    /// <summary>
    /// 新增
    /// </summary>
    public void Create(CurrentUser currentUser)
    {
        if (db.Queryable<Menu>().Any(x => x.parentId == this.parentId && x.name == this.name))
            throw BadRequest("菜单已存在");
        this.creator = currentUser.account;
        db.Insertable(this).ExecuteInsert();
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public void Update(CurrentUser currentUser)
    {
        var menus = db.Queryable<Menu>().Where(x => x.id == this.id || x.parentId == this.parentId && x.name == this.name).ToList();
        if (!menus.Any(x => x.id == this.id))
            throw NotFound("菜单不存在");
        if (menus.Count > 1)
            throw BadRequest("菜单名称已存在");
        db.Updateable(this).ExecuteUpdate(currentUser);
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void Delete(CurrentUser currentUser)
    {
        if (!db.Queryable<Menu>().Any(x => x.id == this.id))
            throw NotFound("菜单不存在");
        if (db.Queryable<Menu>().Any(x => x.parentId == this.id))
            throw BadRequest("含有子菜单，不能直接删除");
        db.Ado.BeginTran();
        db.Updateable(this).ExecuteDelete(currentUser);
        db.Updateable<MenuFunc>().Where(x => x.menuId == this.id).ExecuteDelete(currentUser);
        db.Ado.CommitTran();
    }
}
