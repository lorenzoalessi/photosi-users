namespace PhotosiUsers.Exceptions;

public class UserException : Exception
{
    public UserException()
    {
    }

    public UserException(string message) : base(message)
    {
    }

    public UserException(Exception exception)
    {
    }
}