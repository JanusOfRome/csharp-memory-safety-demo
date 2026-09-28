#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* A tiny stand-in for a network server. It keeps a public message and, right
   after it in memory, a private value. The echo function returns however many
   bytes the client asks for, trusting that number instead of checking it
   against how long the message actually is. This is the shape of the
   Heartbleed bug from 2014. */
struct Packet
{
    char message[16];
    char secret[16];
};

int main(int argc, char *argv[])
{
    struct Packet p;
    strcpy(p.message, "hello");
    strcpy(p.secret, "PIN=4921");

    int requested = atoi(argv[1]);   /* how many bytes the client asks us to echo back */

    printf("Client asked for %d bytes back:\n", requested);
    for (int i = 0; i < requested; i++)
    {
        char c = p.message[i];       /* nothing stops i from running past message */
        putchar(c >= 32 && c < 127 ? c : '.');
    }
    printf("\n");
    return 0;
}
