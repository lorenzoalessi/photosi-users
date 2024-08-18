using System.Diagnostics.CodeAnalysis;

namespace PhotosiUsers.Exceptions;

[ExcludeFromCodeCoverage]
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