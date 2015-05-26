namespace Ast
{
    public interface ICallable
    {
        bool IsArgumentsValid(List args);

        Error GetArgumentError(List args);

        Expression Call(List args);
    }
}

