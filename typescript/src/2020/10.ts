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

  findAllVariations(adapters: number[]) {
    const ALLOWED_OFFSET = 3;
    adapters.sort(numericCompareForSort);

    const start = 0;
    const last = adapters.pop()!;
    const target = last + 3;

    function verify(order: number[]) {
      if (order.length <= 1 || last - order[order.length - 1] > ALLOWED_OFFSET) {
        return false;
      }

      return window(order.concat(last, target), 2)
        .map((c) => c[1] - c[0])
        .every((i) => i > 0 && i <= 3);
    }

    let solutions: number = 0;

    type State = { current: number; order: number[]; leftStartIndex: number };

    let stack: State[] = [{ current: start, order: [], leftStartIndex: 0 }];

    while (stack.length > 0) {
      const state = stack.pop()!;

      if (verify(state.order)) {
        solutions += 1;
      }

      for (let i = state.leftStartIndex; i < adapters.length; i++) {
        this.Part2Bar.increment();

        const option = adapters[i];

        if (option - ALLOWED_OFFSET <= state.current && option - state.current > 0) {
          stack.push({
            current: option,
            order: state.order.concat(option),
            leftStartIndex: i + 1,
          });
        }
      }
    }

    return solutions;
  }

  async part2(input: string): Promise<string> {
    const joltageAdapters = input
      .split("\n")
      .map((n) => parseInt(n))
      .sort(numericCompareForSort);

    this.Part2Bar.setTotal(joltageAdapters.length);

    return this.findAllVariations(joltageAdapters).toString();
  }
}

if (import.meta.main) {
  new Day().execute(false);
}

export default Day;