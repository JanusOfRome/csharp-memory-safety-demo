#include <stdio.h>
#include <string.h>

/* A C version of the bank Account. The name and the balance are two fields
   laid out next to each other in memory. C does not attach a length to the
   name buffer, and copying into it is not checked against its size. */
struct Account
{
    char name[16];
    int balance;
};

int main(int argc, char *argv[])
{
    struct Account acc;
    acc.balance = 100;
    strcpy(acc.name, argv[1]);   /* copies until the source's terminator, not the buffer's size */

    printf("Name:    %s\n", acc.name);
    printf("Balance: %d\n", acc.balance);
    return 0;
}
