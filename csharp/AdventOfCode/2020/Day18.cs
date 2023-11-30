using AdventOfCode.Extensions;
using MoreLinq;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 18)]
public class Day18 : Solution
{
    abstract record SExpr();

    record SExprOperator(string Op) : SExpr{
        public override string ToString() => Op;
    }

    record SExprLiteral(long Value) : SExpr
    {
        public override string ToString() => Value.ToString();
    }

    record SExprList(List<SExpr> Values) : SExpr
    {
        public override string ToString()
        {
            return $"[{Values.ToDelimitedString(",")}]";
        }
    }
    
    private string[] Tokenize(string input) =>
        $"({input})"
            .Replace("(", " ( ")
            .Replace(")", " ) ")
            .Trim()
            .Split(" ")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

    /**
      There is no operator precedence.
      <code>
       1 + (2 * 3) + (4 * (5 + 6))
       1 +    6    + (4 * (5 + 6))
       7      + (4 * (5 + 6))
       7      + (4 *   11   )
       7      +     44
       51
       </code>
       <code>
       1 + (2 * 3) + (4 * (5 + 6)) should become (+ (+ 1 (* 2 3)) (* 4 (+ 6 5)))
       </code>
       ( -- List
        1 + (2 * 3) + (4 * (5 + 6)))
     */
    private (SExpr, int) Parse(string[] input, int index = 0)
    {
        var token = input[index];
        index += 1;

        if (token != "(")
            throw new Exception($"Invalid input: {input[index..].ToDelimitedString(" ")}");

        var expressions = new List<SExpr>();
        
        while (true)
        {
            token = input[index];

            if (token == ")")
            {
                index++;
                break;
            }

            if (token == "(")
            {
                var (sexpr, updatedIndex) = Parse(input, index);
                expressions.Add(sexpr);

                index = updatedIndex;
            }
            else if (long.TryParse(token, out var digit))
            {
                expressions.Add(new SExprLiteral(digit));
                index++;
            }
            else if (token is "+" or "*")
            {
                expressions.Add(new SExprOperator(token));
                index++;
            }
            else
            {
                throw new Exception($"Unknown Token: {token}");
            }
        }

        return (new SExprList(expressions), index);
    }

    private long Interpret(SExpr expr)
    {
        if (expr is SExprLiteral literal) return literal.Value;
        if (expr is SExprList list)
        {
            if (list.Values.Count == 1) 
                return Interpret(list.Values[0]);
            
            var sum = 0L;
            var prev = 0L;
            
            for (var i = 0; i < list.Values.Count; i++)
            {
                var first = list.Values[i];
                if (first is SExprOperator @operator)
                {
                    var next = Interpret(list.Values[++i]);

                    if (@operator.Op == "+")
                    {
                        sum = prev + next;
                    }
                    else
                    {
                        sum = prev * next;
                    }

                    prev = sum;
                }
                else
                {
                    prev = Interpret(first);
                }
            }

            return sum;
        }
        throw new Exception("Cannot eval operator");
    }

    public override Answer Part1(string input)
    {
        var lines = input.SplitLines();

        var evals = new List<long>();
        foreach (var (i, line) in lines.Index())
        {
            var (expr, idx) = Parse(Tokenize(line));
            var sum = Interpret(expr);
            evals.Add(sum);
        }

        return evals.Sum();
    }

    private SExpr OverridePrecedence(SExpr expr)
    {
        if (expr is SExprList list)
        {
            var updatedList = new List<SExpr>();
            SExpr? prev = null;
            
            for (int i = 0; i < list.Values.Count; i++)
            {
                var first = list.Values[i];
                if (first is SExprOperator { Op: "+" })
                {
                    var newGroup = new SExprList(new() { OverridePrecedence(prev!), first, OverridePrecedence(list.Values[++i]) });

                    updatedList = updatedList.SkipLast(1).ToList();
                    
                    updatedList.Add(newGroup);
                    prev = newGroup;
                }
                else
                {
                    prev = first;
                    updatedList.Add(OverridePrecedence(first));
                }
            }

            return new SExprList(updatedList);
        }

        return expr;
    }
    
    public override Answer Part2(string input)
    {
        var lines = input.SplitLines();

        var evals = new List<long>();
        foreach (var (i, line) in lines.Index())
        {
            var (expr, idx) = Parse(Tokenize(line));
            var sum = Interpret(OverridePrecedence(expr));
            
            evals.Add(sum);
        }

        return evals.Sum();
    }
}