namespace Application.Exceptions;

public class InvalidArgumentException : Exception
{
    public InvalidArgumentException(string massage) : base(massage)
    {
        
    }
}