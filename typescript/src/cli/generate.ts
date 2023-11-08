import { A } from "@mobily/ts-belt";

const args = A.slice(Bun.argv, 2, 2);

const year = parseInt(args[0]);
const day = parseInt(args[1]);
const dayPadded = day.toString().padStart(2, "0");
const file = `./src/${year}/${dayPadded}`;

confirm(`Create ${file}.ts?`);

const dayTempl = `
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(${year}, ${day});
  }

  async part1(input: string): Promise<string> { return "TODO"; }
  async part2(input: string): Promise<string> { return "TODO"; }
}

if (import.meta.main) {
    new Day().execute();
}

export default Day;
`.trim();

Bun.write(file + ".ts", dayTempl);

const testTempl = `
import { expect, test } from "bun:test";
import Solution from "./${dayPadded}";

const solution = new Solution();
const input = await solution.input();

test("Part #1", async () => {
  expect(await solution.part1(input)).toBe("Solution #1");
});

test("Part #2", async () => {
  expect(await solution.part2(input)).toBe("Solution #2");
});
`.trim();

Bun.write(file + ".spec.ts", testTempl);
