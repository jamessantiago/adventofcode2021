#include <stdio.h>
#include <stdlib.h>

int main()
{
  FILE *f = NULL;
  char buf[256];
  int last[3] = {0};
  int day1a = 0;
  int day1b = 0;

  f = fopen("01.txt", "r");
  if (f == NULL) goto done;

  while (fgets(buf, 256, f) != NULL) {
    int cur = atoi(buf);
    if (last[0] > 0 && cur > last[0]) day1a++;
    if (last[2] > 0 && cur > last[2]) day1b++;
    last[2] = last[1]; last[1] = last[0]; last[0] = cur;
  }

  printf("Day 01: %d\n", day1a);
  printf("Day 02: %d\n", day1b);

done:
  if (f) fclose(f);
}
