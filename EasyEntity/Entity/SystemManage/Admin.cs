
using EasyCommon;
using SqlSugar;

namespace EasyEntity.Entity;

public class Admin : AdminEntity
{
    public int id { get; init; }
    public string account { get; private set; }
    public string name { get; private set; }
    public string pwd { get; private set; }
    public string email { get; private set; }
    public string memo { get; private set; }

    [SugarColumn(IsJson = true)]
    public List<int> roles { get; private set; }

    public static Admin Get(int id)
    {
        return db.Queryable<Admin>().First(x => x.id == id);
    }
}
