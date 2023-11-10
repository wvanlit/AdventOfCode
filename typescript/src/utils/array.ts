/**
 * Creates an array of arrays, each containing a "window" of elements from the input array.
 * @param arr - The input array.
 * @param size - The size of the window.
 * @returns An array of windows.
 */
export function window<T>(arr: T[], size: number): T[][] {
  const result: T[][] = [];
  for (let i = 0; i <= arr.length - size; i++) {
    result.push(arr.slice(i, i + size));
  }

  return result;
}

export function max(arr: number[]): number {
  return arr.reduce((p, c) => (c > p ? c : p));
}

export function sum(arr: number[]): number {
  return arr.reduce((p, c) => p + c, 0);
}

export function sortMutable(arr: number[]): number[] {
  return arr.sort((a, b) => a - b);
}

/**
 * Immutable sort, does not touch the original array
 * @returns a new array, sorted
 */
export function sort(arr: number[]): number[] {
  const arrCopy = [...arr];
  return arrCopy.sort((a, b) => a - b);
}

export function permute<T>(arr: T[], n: number): T[][] {
  if (n > arr.length) {
    throw new Error("N cannot be larger than the array length");
  }

  function helper(startIndex: number, temp: T[]): void {
    if (temp.length === n) {
      result.push([...temp]);
      return;
    }

    for (let i = startIndex; i < arr.length; i++) {
      temp.push(arr[i]);
      helper(i + 1, temp);
      temp.pop();
    }
  }

  const result: T[][] = [];
  helper(0, []);
  return result;
}
