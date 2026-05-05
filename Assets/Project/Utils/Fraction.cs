using System;

[Serializable]
public struct Fraction
{
    public int Numerator;
    public int Denominator;

    public Fraction(int n, int d)
    {
        if (d == 0) d = 1;
        Numerator = n;
        Denominator = d;
    }

    public static Fraction Add(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.Numerator * f2.Denominator + f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator).Simplify();
    }

    public static Fraction Subtract(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.Numerator * f2.Denominator - f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator).Simplify();
    }

    public static Fraction Multiply(Fraction f1, Fraction f2)
    {
        return new Fraction(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator).Simplify();
    }

    public Fraction Simplify()
    {
        int gcd = GetGCD(Math.Abs(Numerator), Math.Abs(Denominator));
        return new Fraction(Numerator / gcd, Denominator / gcd);
    }

    private static int GetGCD(int a, int b)
    {
        while (b != 0)
        {
            int t = b;
            b = a % b;
            a = t;
        }
        return a;
    }

    public override string ToString()
    {
        if (Numerator % Denominator == 0)
        {
            return $"{Numerator / Denominator}";
        }
        return $"<sup>{Numerator}</sup>/<sub>{Denominator}</sub>";
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Fraction)) return false;
        Fraction other = (Fraction)obj;
        return Numerator == other.Numerator && Denominator == other.Denominator;
    }

    public override int GetHashCode()
    {
        return Numerator.GetHashCode() ^ Denominator.GetHashCode();
    }

    public static bool operator ==(Fraction f1, Fraction f2) => f1.Equals(f2);
    public static bool operator !=(Fraction f1, Fraction f2) => !f1.Equals(f2);
}
