namespace EasyService;

public class BaseService
{
    public BadRequestException BadRequestExp(string msg) => new(msg);
}

public class BadRequestException : Exception
{
    public BadRequestException(string msg) : base(msg)
    {

    }
}
