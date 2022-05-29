
namespace EasyEntity.Entity;

public class Admin : AdminEntity
{
    public long id { get; init; }
    public string account { get; private set; }
    public string name { get; private set; }
    public string pwd { get; private set; }
    public string email { get; private set; }
    public string memo { get; private set; }
    public List<int> roles { get; private set; }
    public bool oust { get; private set; }

    public static Admin Get(long id)
    {
        return db.Queryable<Admin>().First(x => x.id == id);
    }
}
