import { A, S, pipe } from "@mobily/ts-belt";
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 1);
  }

  async part1(input: string): Promise<string> {
    const entries = S.split(input, "\n").map((s) => parseInt(s));
    const pairs = entries.flatMap((n) => entries.map((m) => [n, m] as const));

    this.Part1Bar.setTotal(pairs.length);

    const result = pipe(
      pairs,
      A.filter((v) => v[0] !== v[1]),
      A.tap((_) => this.Part1Bar.increment()),
      A.find((v) => v[0] + v[1] === 2020)
    )!;

    return (result[0] * result[1]).toString();
  }

  async part2(input: string): Promise<string> {
    const entries = S.split(input, "\n").map((s) => parseInt(s));
    const pairs = entries.flatMap((n) => entries.flatMap((m) => entries.map((o) => [n, m, o] as const)));

    this.Part2Bar.setTotal(pairs.length);

    const result = pipe(
      pairs,
      A.filter((v) => v[0] !== v[1] && v[0] !== v[2]),
      A.tap((_) => this.Part2Bar.increment()),
      A.find((v) => v[0] + v[1] + v[2] === 2020)
    )!;

    return (result[0] * result[1] * result[2]).toString();
  }
}

if (import.meta.main) {
  new Day().execute();
}

export default Day;
