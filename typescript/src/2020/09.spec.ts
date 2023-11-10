import { expect, test } from "bun:test";
import Solution from "./09";

const solution = new Solution();
const input = await solution.input();

test("Part #1", async () => {
  expect(await solution.part1(input)).toBe("57195069");
});

test("Part #2", async () => {
  expect(await solution.part2(input)).toBe("7409241");
});
