
[Flags]
internal enum Features
{
    None = 0,
    Brakes = 1,
    Radio = 2,
    AirConditioning = 4
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // a method that converts a number to binary
        Action<int> b = n => Console.WriteLine(Convert.ToString(n, 2).PadLeft(8, '0'));

        b(1); //output - 00000001
        b(1 + 1); //output - 00000010

        var v = 0b1010;
        var x = 0b0110;
        b(v & x); // 'and' output - 00000010
        b(v | x); // 'or' output - 00001110
        b(v ^ x); // 'xor' output - 00001100

        var hasAC = Features.AirConditioning;
        var hasBrakes = Features.Brakes;

        if (hasAC == Features.AirConditioning && hasBrakes == Features.Brakes)
        {
            // Do some work
        }

        var features = Features.AirConditioning | Features.Brakes;
        Console.WriteLine(features);
        b((int)features);

        if ((features & Features.Brakes) == Features.Brakes)
        {
            Console.WriteLine("Has Brakes");
        }
    }
}
