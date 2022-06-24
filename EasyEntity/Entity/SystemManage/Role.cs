using EasyCommon;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEntity.Entity;

public class Role : AdminEntity
{
    public string name { get; private set; }
    public string memo { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<long> menus { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<long> funcs { get; private set; }

    /// <summary>
    /// 新增
    /// </summary>
    public void Create(CurrentUser currentUser)
    {
        if (db.Queryable<Role>().Any(x => x.name == this.name))
            throw BadRequest("角色已存在");
        this.creator = this.updator = currentUser.account;
        db.Insertable(this).ExecuteCommand();
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public void Update(CurrentUser currentUser)
    {
        var roles = db.Queryable<Role>().Where(x => x.id == this.id || x.name == this.name).ToList();
        if (!roles.Any(x => x.id == this.id))
            throw NotFound("角色不存在");
        if (roles.Count > 1)
            throw BadRequest("角色名称已存在");
        this.updator = currentUser.account;
        db.Updateable(this).ExecuteCommand();
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void Delete(CurrentUser currentUser)
    {
        if (!db.Queryable<Role>().Any(x => x.id == this.id))
            throw NotFound("角色不存在");
        this.updator = currentUser.account;
        db.Deleteable(this).IsLogic().ExecuteCommand(this);
    }
}
