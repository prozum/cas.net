namespace Ast
{
    // A BinaryOperator which sides can be swapped without effecting the result.
    public interface ISwappable
    {
        Expression Left { get; set; }
        Expression Right { get; set; }

        BinaryOperator Swap();
        BinaryOperator Transform();
        string ToStringParent();
    }
}
