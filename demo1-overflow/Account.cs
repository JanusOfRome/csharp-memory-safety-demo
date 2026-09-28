using System;

// The same shape as the C struct: a fixed-size name buffer and a balance,
// as close to the C layout as C# lets us get. The point is what happens when
// we copy a name that is too long for the buffer.
class Account
{
    private char[] name = new char[16];
    private int balance = 100;

    // Mirrors the C strcpy: copy the source in character by character. There
    // is no manual length check here, on purpose, to match the C version.
    public void SetName(string source)
    {
        for (int i = 0; i < source.Length; i++)
        {
            name[i] = source[i];
        }
    }

    public void Print()
    {
        Console.WriteLine("Name:    " + new string(name).TrimEnd('\0'));
        Console.WriteLine("Balance: " + balance);
    }

    static void Main(string[] args)
    {
        Account acc = new Account();

        try
        {
            acc.SetName(args[0]);
            acc.Print();
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine("Stopped by the runtime: " + e.Message);
            acc.Print();
        }
    }
}
