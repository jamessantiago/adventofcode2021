#include <string.h>
#include <stdio.h>
#include <stdlib.h>

const int FORWARD = 1;
const int DOWN = 2;
const int UP = 3;

static void
parse (char *str, int *out)
{
  char *t;
  t = strtok (str, " ");
  if (strcmp (t, "forward") == 0)
    {
      out[0] = FORWARD;
    }
  else if (strcmp (t, "down") == 0)
    {
      out[0] = DOWN;
    }
  else
    {
      out[0] = UP;
    }
  t = strtok (NULL, " ");
  out[1] = atoi (t);
}

int
main ()
{
  FILE *f = NULL;
  char buf[256];
  int result1[2] = { 0 };
  int result2[3] = { 0 };
  int *pbuf = NULL;
  
  pbuf = malloc (2);
  f = fopen ("02.txt", "r");
  if (pbuf == NULL || f == NULL) goto done;

  while (fgets (buf, 256, f))
    {
      parse (buf, pbuf);
      if (pbuf[0] == FORWARD)
	{
	  result1[0] += pbuf[1];
	  result2[0] += pbuf[1];
	  result2[1] += pbuf[1] * result2[2];
	}
      else if (pbuf[0] == DOWN)
	{
	  result1[1] += pbuf[1];
	  result2[2] += pbuf[1];
	}
      else
	{
	  result1[1] -= pbuf[1];
	  result2[2] -= pbuf[1];
	}
    }

  printf ("Result 1: %d\n", result1[0] * result1[1]);
  printf ("Result 2: %d\n", result2[0] * result2[1]);

done:
  if (f) fclose (f);
  if (pbuf) free(pbuf);
}
