using System;
using System.Threading;

// The Account from the bank system. Withdraw checks the balance first, then
// subtracts. That check-then-act is safe on its own, but not when many threads
// run it on the same account at the same time.
class Account
{
    private int _balance;
    private readonly object _gate = new object();

    public Account(int balance) { _balance = balance; }
    public int Balance { get { return _balance; } }

    // The original single-threaded version from Task 3.2.
    public bool Withdraw(int amount)
    {
        if (amount <= 0 || amount > _balance)
        {
            return false;
        }

        // _balance -= amount is really three steps: read, subtract, write.
        // Written out, so the gap where another thread can read the same old
        // value before we write ours back is visible. Yield makes it reliable.
        int current = _balance;
        Thread.Yield();
        _balance = current - amount;
        return true;
    }

    // The fix: lock makes the check and the subtraction one indivisible step,
    // so a second thread cannot slip in between them.
    public bool WithdrawSafely(int amount)
    {
        lock (_gate)
        {
            if (amount <= 0 || amount > _balance)
            {
                return false;
            }

            _balance -= amount;
            return true;
        }
    }
}

class Program
{
    // 2000 threads each try to withdraw 1 from an account holding 100. The
    // account only has enough for 100 of them, so at most 100 must succeed.
    static void Run(string label, Func<Account, int, bool> withdraw)
    {
        Account acc = new Account(100);
        int succeeded = 0;
        Barrier gate = new Barrier(2000);

        Thread[] threads = new Thread[2000];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                gate.SignalAndWait();          // all 2000 start together
                if (withdraw(acc, 1))
                {
                    Interlocked.Increment(ref succeeded);
                }
            });
            threads[i].Start();
        }
        foreach (Thread t in threads) t.Join();

        Console.WriteLine(label);
        Console.WriteLine("  withdrawals allowed: " + succeeded + "  (the account only held 100)");
        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("Account holds 100. 2000 threads each withdraw 1 at the same time.");
        Console.WriteLine("A correct account allows at most 100 of them.");
        Console.WriteLine();

        Run("Unlocked Withdraw:", (a, amt) => a.Withdraw(amt));
        Run("Locked WithdrawSafely:", (a, amt) => a.WithdrawSafely(amt));
    }
}
