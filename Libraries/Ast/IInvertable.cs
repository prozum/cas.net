namespace Ast
{
    public interface IInvertable
    {
        Expression InvertOn(Expression other);
    }
}
