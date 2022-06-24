global using static EasyCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasyEntity.Entity;

public class Menu : AdminEntity
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
        this.creator = this.updator = currentUser.account;
        db.Insertable(this).ExecuteCommand();
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
        this.updator = currentUser.account;
        db.Updateable(this).ExecuteCommand();
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
        this.updator = currentUser.account;
        db.Deleteable(this).IsLogic().ExecuteCommand(this);
    }
}
