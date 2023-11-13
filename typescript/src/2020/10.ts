import { numericCompareForSort, window } from "../utils/array";
import { factorial } from "../utils/math";
import { memoize } from "../utils/memo";
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 10);
  }

  /*
   *  Each adapter is rated for a specific output joltage
   *  Any given adapter can take an input 1, 2, or 3 jolts lower than its rating
   *  Device has a adapter rated for 3 jolts higher than the highest-rated adapter in your bag
   *  The charging outlet has a joltage rating of 0.
   */

  async part1(input: string): Promise<string> {
    const joltageAdapters = input
      .split("\n")
      .map((n) => parseInt(n))
      .sort(numericCompareForSort);

    const allAdapters = [0 /* Outlet */, ...joltageAdapters, joltageAdapters.at(-1)! + 3 /* Device */];
    const diffs = window(allAdapters, 2).map((c) => c[1] - c[0]);

    const diffsOf1 = diffs.filter((i) => i === 1).length;
    const diffsOf3 = diffs.filter((i) => i === 3).length;

    return (diffsOf1 * diffsOf3).toString();
  }

  isValid = (adapters: number[]) => {
    if (adapters.length <= 1) return true;

    const diff = adapters.at(-1)! - adapters.at(-2)!;

    if (diff < 0 && diff > 3) return false;

    const diffs = window(adapters, 2).map((c) => c[1] - c[0]);

    return diffs.every((i) => i > 0 && i <= 3);
  };

  isSolution = (order: number[], target: number) => {
    if (target - order.at(-1)! > 3) {
      return false;
    }

    const solution = order.concat(target);
    const diffs = window(solution, 2).map((c) => c[1] - c[0]);

    return diffs.every((i) => i > 0 && i <= 3);
  };

  findAllVariations(adapters: number[]) {
    const target = adapters.at(-1)! + 3;

    let cache: Record<string, number> = {};
    function toHash(current: number, next: number) {
      return `${current}_${next}`;
    }

    const backtrack = (order: number[], options: number[]) => {
      let solutions: number = 0;
      let hash = toHash(order.at(-1)!, options[0]);

      if (cache[hash]) {
        return cache[hash];
      }

      if (this.isSolution(order, target)) {
        solutions++;
      }

      for (const option of options.sort()) {
        order.push(option);

        // Prune invalid searches
        if (this.isValid(order)) {
          this.Part2Bar.increment();
          solutions += backtrack(
            order,
            options.filter((i) => i !== option)
          );
        } else {
          this.Part2Bar.increment(factorial(options.length - 1));
        }

        order.pop();
      }

      cache[hash] = solutions;

      return solutions;
    };

    return backtrack([0], adapters);
  }

  async part2(input: string): Promise<string> {
    const joltageAdapters = input
      .split("\n")
      .map((n) => parseInt(n))
      .sort(numericCompareForSort);

    this.Part2Bar.setTotal(factorial(joltageAdapters.length));

    return this.findAllVariations(joltageAdapters).toString();
  }
}

if (import.meta.main) {
  new Day().execute(false);
}

export default Day;
