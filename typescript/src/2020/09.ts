import { permute, sum, window } from "../utils/array";
import { Solution } from "../utils/solution";

class Day extends Solution {
  PREAMBLE_SIZE = 25;

  constructor() {
    super(2020, 9);
  }

  async part1(input: string): Promise<string> {
    const numbers = input.split("\n").map((n) => parseInt(n));

    const values = numbers.slice(this.PREAMBLE_SIZE);
    let index = 0;

    for (const value of values) {
      const preamble = numbers.slice(index, index + this.PREAMBLE_SIZE);

      const hasCombination = permute(preamble, 2).some((c) => c[0] + c[1] == value);

      if (!hasCombination) return value.toString();

      index++;
    }

    return "Failed!";
  }

  async part2(input: string): Promise<string> {
    const numbers = input.split("\n").map((n) => parseInt(n));
    const target = parseInt(await this.part1(input));

    const MAX_CONTIGUOUS_SIZE = 20;

    for (let sizeOfSet = 2; sizeOfSet <= MAX_CONTIGUOUS_SIZE; sizeOfSet++) {
      const set = window(numbers, sizeOfSet).find((set) => sum(set) === target);

      if (set !== undefined) {
        set.sort();
        return (set.at(0)! + set.at(-1)!).toString();
      }
    }

    return "Failed!";
  }
}

if (import.meta.main) {
  new Day().execute(true);
}

export default Day;
