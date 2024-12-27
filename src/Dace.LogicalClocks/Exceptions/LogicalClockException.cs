namespace Dace.LogicalClocks.Exceptions;

using System;

public class LogicalClockException : Exception
{
    protected LogicalClockException(
        string message) 
        : base(message)
    {
    }

    protected LogicalClockException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
    protected LogicalClockException()
        : base()
    {

    }
}
