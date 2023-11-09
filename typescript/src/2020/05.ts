import { max, sort, window } from "../utils/array";
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 5);
  }

  calculateSeatId(seatCode: string) {
    const rowCode = seatCode.slice(0, 7).split("");
    const columnCode = seatCode.slice(7).split("");

    const calculateRow = () => {
      let min = 0;
      let max = 127;
      let half = 64;

      for (const letter of rowCode) {
        if (letter === "F") {
          max -= half;
        } else if (letter === "B") {
          min += half;
        }

        half = half / 2;
      }

      console.assert(min === max, "Row: Min should equal Max");

      return min;
    };

    const calculateColumn = () => {
      let min = 0;
      let max = 7;
      let half = 4;

      for (const letter of columnCode) {
        if (letter === "L") {
          max -= half;
        } else if (letter === "R") {
          min += half;
        }

        half = half / 2;
      }

      console.assert(min === max, "Column: Min should equal Max");

      return min;
    };

    const row = calculateRow();
    const column = calculateColumn();

    return row * 8 + column;
  }

  async part1(input: string): Promise<string> {
    const lines = input.split("\n");
    this.Part1Bar.setTotal(lines.length);

    const seats = lines.map((l) => {
      this.Part1Bar.increment();
      return this.calculateSeatId(l);
    });

    return max(seats).toString();
  }

  async part2(input: string): Promise<string> {
    const lines = input.split("\n");
    this.Part2Bar.setTotal(lines.length * 2); // Because we need to go over everything twice

    const seats = lines.map((l) => {
      this.Part2Bar.increment();
      return this.calculateSeatId(l);
    });

    const sorted = sort(seats);
    const pairs = window(sorted, 2);

    const missingSeat = pairs.find((p) => {
      this.Part2Bar.increment();
      return p[1] - p[0] !== 1;
    })!;

    const id = missingSeat[1] - 1;

    return id.toString();
  }
}

if (import.meta.main) {
  new Day().execute();
}

export default Day;
