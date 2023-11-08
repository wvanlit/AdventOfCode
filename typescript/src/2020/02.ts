import { xor } from "../utils/bool";
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 2);
  }

  parse(s: string) {
    const re = /(?<min>\d+)-(?<max>\d+) (?<char>\w): (?<password>\w+)/;
    const g = s.match(re)?.groups!;

    return { min: parseInt(g.min), max: parseInt(g.max), char: g.char, password: g.password };
  }

  async part1(input: string): Promise<string> {
    const policies = input
      .trim()
      .split("\n")
      .map((s) => this.parse(s));

    this.Part1Bar.setTotal(policies.length);

    let valid = 0;

    policies.forEach((policy) => {
      this.Part1Bar.increment();

      const occurrences = policy.password.split("").filter((c) => c === policy.char).length;

      if (occurrences >= policy.min && occurrences <= policy.max) {
        valid++;
      }
    });

    return valid.toString();
  }

  async part2(input: string): Promise<string> {
    const policies = input
      .trim()
      .split("\n")
      .map((s) => this.parse(s));

    this.Part2Bar.setTotal(policies.length);

    let valid = 0;

    policies.forEach((policy) => {
      this.Part2Bar.increment();
      const chars = policy.password.split("");

      const matchMin = chars[policy.min - 1] === policy.char;
      const matchMax = chars[policy.max - 1] === policy.char;

      if (xor(matchMin, matchMax)) {
        valid++;
      }
    });

    return valid.toString();
  }
}

if (import.meta.main) {
  new Day().execute();
}

export default Day;
