namespace AdventOfCode.Utils;

public static class Modulo
{
    /// <summary>
    /// Solves a system of congruences using the Chinese Remainder Theorem.
    /// </summary>
    /// <param name="input">A list of tuples, each containing a remainder and a modulus.</param>
    /// <returns>The smallest positive integer that satisfies all the given congruences.</returns>
    public static long ChineseRemainderTheorem(List<(long remainder, long modulus)> input)
    {
        var product = input.Select(t => t.modulus).Aggregate(1L, (prev, curr) => prev * curr);

        var result = 0L;
        foreach (var (remainder, modulus) in input)
        {
            var p = product / modulus;

            result += remainder * ModularInverse(p, modulus) * p;
        }

        return result % product;
    }

    /// <summary>
    /// Calculates the modular inverse of a value with respect to a modulus.
    /// </summary>
    /// <param name="value">The value to find the modular inverse for.</param>
    /// <param name="modulus">The modulus to use for calculation.</param>
    /// <returns>The modular inverse of the value.</returns>
    /// <exception cref="Exception">Thrown when the modular inverse does not exist.</exception>
    /// <remarks>
    /// A modular inverse of 'a' modulo 'm' is a number 'b' such that (a * b) % m = 1.
    /// </remarks>
    public static long ModularInverse(long value, long modulus)
    {
        var (g, x, _) = ExtendedGreatestCommonDivisor(value, modulus);
        if (g != 1) throw new Exception("Modular Inverse does not exist!");
        return (x % modulus + modulus) % modulus; // Ensure the result is positive
    }

    /// <summary>
    /// Computes the Extended Greatest Common Divisor of two numbers.
    /// </summary>
    /// <remarks>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>A tuple containing the gcd and the coefficients of Bézout's identity.</returns>
    /// Bézout's Identity states that for any integers 'a' and 'b',
    /// there exist integers 'x' and 'y' such that a * x + b * y = gcd(a, b).
    /// </remarks>
    public static (long g, long x, long y) ExtendedGreatestCommonDivisor(long a, long b)
    {
        if (a == 0) return (b, 0, 1);
        var (g, y, x) = ExtendedGreatestCommonDivisor(b % a, a);
        return (g, x - (b / a) * y, y);
    }
}