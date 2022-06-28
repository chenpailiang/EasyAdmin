
using EasyCommon;
using SqlSugar;
using System.Text;

namespace EasyEntity.Entity;

public class Admin : BaseEntity
{
    public string account { get; private set; }
    public string name { get; private set; }
    public string pwd { get; private set; }
    public string email { get; private set; }
    public string memo { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<long>? roles { get; private set; }

    /// <summary>
    /// 新增
    /// </summary>
    public void Create(CurrentUser currentUser)
    {
        if (db.Queryable<Admin>().Any(x => x.account == this.account))
            throw BadRequest("账号已存在");
        var letters = "qwertyuiopasdfghjklzxcvbnm";
        var nums = "1234567890";
        var chars = ",./!@#$&-+";
        var sb = new StringBuilder();
        var rand = new Random();
        for (int i = 0; i < 5; i++)
        {
            var idx = rand.Next(0, 26);
            sb.Append(letters[idx]);
        }
        for (int i = 0; i < 3; i++)
        {
            var idx = rand.Next(0, 10);
            sb.Append(nums[idx]);
        }
        for (int i = 0; i < 2; i++)
        {
            var idx = rand.Next(0, 10);
            sb.Append(chars[idx]);
        }
        this.pwd = sb.ToString();
        this.creator = this.updator = currentUser.account;
        db.Insertable(this).IgnoreColumns(x => x.roles).ExecuteCommand();
        EmailTool.SendEmailForNewAdmin(this.email, this.account, this.name, this.pwd);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public void Update(CurrentUser currentUser)
    {
        var admin = db.Queryable<Admin>().First(x => x.id == this.id);
        if (admin == null)
            throw NotFound("账号不存在");
        this.updator = currentUser.account;
        db.Updateable(this).UpdateColumns(x => new { x.name, x.email, x.memo, x.updator }).ExecuteCommand();
    }

    /// <summary>
    /// 删除
    /// </summary>
    public void Delete(CurrentUser currentUser)
    {
        var admin = db.Queryable<Admin>().First(x => x.id == this.id);
        if (admin == null)
            throw NotFound("账号不存在");
        db.Deleteable(this).IsLogic().ExecuteDelete(currentUser.account);
    }
}
