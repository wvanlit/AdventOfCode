import { memoize } from "./memo";

export const factorial = memoize((n: number): number => {
  if (n === 0 || n === 1) {
    return 1;
  } else {
    return n * factorial(n - 1);
  }
});
