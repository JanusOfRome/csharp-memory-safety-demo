using System;

// The same server in C#. The message and the secret are separate array
// objects on the heap, not two fields laid out next to each other. And every
// read of the message is checked against its length. So the over-read fails
// for two reasons at once.
class Server
{
    static char[] message = new char[16];
    static char[] secret = new char[16];

    static void Main(string[] args)
    {
        "hello".CopyTo(0, message, 0, 5);
        "PIN=4921".CopyTo(0, secret, 0, 8);

        int requested = int.Parse(args[0]);
        Console.WriteLine("Client asked for " + requested + " bytes back:");

        try
        {
            for (int i = 0; i < requested; i++)
            {
                char c = message[i];
                Console.Write(c >= 32 && c < 127 ? c : '.');
            }
            Console.WriteLine();
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine();
            Console.WriteLine("Stopped by the runtime: " + e.Message);
        }
    }
}
