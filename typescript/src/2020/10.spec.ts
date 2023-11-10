import { expect, test } from "bun:test";
import Solution from "./10";

const solution = new Solution();
const input = await solution.input();

test("Part #1", async () => {
  expect(await solution.part1(input)).toBe("Solution #1");
});

test("Part #2", async () => {
  expect(await solution.part2(input)).toBe("Solution #2");
});