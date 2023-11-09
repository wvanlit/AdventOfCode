import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 3);
  }

  async part1(input: string): Promise<string> {
    const map = input
      .split("\n")
      .map((s) => s.split(""))
      .slice(1);

    let position = 0;
    let trees = 0;

    this.Part1Bar.setTotal(map.length);

    for (let row = 0; row < map.length; row++) {
      const line = map[row];
      position = (position + 3) % line.length;
      if (line[position] === "#") trees++;

      this.Part1Bar.increment();
    }

    return trees.toString();
  }

  async part2(input: string): Promise<string> {
    const slopes = [
      [1, 1],
      [3, 1],
      [5, 1],
      [7, 1],
      [1, 2],
    ] as const;

    this.Part2Bar.setTotal(slopes.length);

    const trees = slopes.map((s) => {
      const right = s[0];
      const down = s[1];

      const map = input
        .split("\n")
        .map((s) => s.split(""))
        .slice(down);

      let position = 0;
      let trees = 0;

      for (let row = 0; row < map.length; row += down) {
        const line = map[row];
        position = (position + right) % line.length;
        if (line[position] === "#") trees++;
      }

      this.Part2Bar.increment();

      return trees;
    });

    return trees.reduce((p, c) => p * c).toString();
  }
}

if (import.meta.main) {
  new Day().execute();
}

export default Day;
