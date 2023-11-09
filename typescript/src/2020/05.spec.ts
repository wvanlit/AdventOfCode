import { describe, expect, test } from "bun:test";
import Solution from "./05";

const solution = new Solution();
const input = await solution.input();

describe("SeatId calculation", () => {
  test("BFFFBBFRRR", () => expect(solution.calculateSeatId("BFFFBBFRRR")).toBe(567));
  test("FFFBBBFRRR", () => expect(solution.calculateSeatId("FFFBBBFRRR")).toBe(119));
  test("BBFFBBFRLL", () => expect(solution.calculateSeatId("BBFFBBFRLL")).toBe(820));
});

test("Part #1", async () => {
  expect(await solution.part1(input)).toBe("885");
});

test("Part #2", async () => {
  expect(await solution.part2(input)).toBe("623");
});
