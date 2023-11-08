import { A } from "@mobily/ts-belt";

/**
 * Creates an array of arrays, each containing a "window" of elements from the input array.
 * @param arr - The input array.
 * @param size - The size of the window.
 * @returns An array of windows.
 */
export function window<T>(arr: T[], size: number): T[][] {
  const range = A.range(0, A.length(arr) - size);
  const sliced = range.map((start) => A.slice(arr, start, start + size));

  return sliced as T[][];
}
