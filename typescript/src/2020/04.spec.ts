import { expect, test, describe } from "bun:test";
import Solution from "./04";

const solution = new Solution();
const input = await solution.input();

test("Part #1", async () => {
  expect(await solution.part1(input)).toBe("254");
});

test("Part #2", async () => {
  expect(await solution.part2(input)).toBe("184");
});

describe("Passport validators", () => {
  // Birth Year Tests
  describe("matchBirthYear", () => {
    test("valid birth year", () => {
      expect(solution.matchBirthYear("byr:1987")).toBe(true);
    });
    test("birth year too low", () => {
      expect(solution.matchBirthYear("byr:1919")).toBe(false);
    });
    test("birth year too high", () => {
      expect(solution.matchBirthYear("byr:2003")).toBe(false);
    });
    test("invalid birth year format", () => {
      expect(solution.matchBirthYear("byr:19a7")).toBe(false);
    });
  });

  // Issue Year Tests
  describe("matchIssueYear", () => {
    test("valid issue year", () => {
      expect(solution.matchIssueYear("iyr:2015")).toBe(true);
    });
    test("issue year too low", () => {
      expect(solution.matchIssueYear("iyr:2009")).toBe(false);
    });
    test("issue year too high", () => {
      expect(solution.matchIssueYear("iyr:2021")).toBe(false);
    });
    test("invalid issue year format", () => {
      expect(solution.matchIssueYear("iyr:20b5")).toBe(false);
    });
  });

  // Expiration Year Tests
  describe("matchExpirationYear", () => {
    test("valid expiration year", () => {
      expect(solution.matchExpirationYear("eyr:2025")).toBe(true);
    });
    test("expiration year too low", () => {
      expect(solution.matchExpirationYear("eyr:2019")).toBe(false);
    });
    test("expiration year too high", () => {
      expect(solution.matchExpirationYear("eyr:2031")).toBe(false);
    });
    test("invalid expiration year format", () => {
      expect(solution.matchExpirationYear("eyr:202x")).toBe(false);
    });
  });

  // Height Tests
  describe("matchHeight", () => {
    test("valid height in cm", () => {
      expect(solution.matchHeight("hgt:170cm")).toBe(true);
    });
    test("valid height in in", () => {
      expect(solution.matchHeight("hgt:65in")).toBe(true);
    });
    test("height too low in cm", () => {
      expect(solution.matchHeight("hgt:149cm")).toBe(false);
    });
    test("height too high in cm", () => {
      expect(solution.matchHeight("hgt:194cm")).toBe(false);
    });
    test("height too low in in", () => {
      expect(solution.matchHeight("hgt:58in")).toBe(false);
    });
    test("height too high in in", () => {
      expect(solution.matchHeight("hgt:77in")).toBe(false);
    });
    test("invalid height format", () => {
      expect(solution.matchHeight("hgt:170")).toBe(false);
    });
  });

  // Hair Color Tests
  describe("matchHairColor", () => {
    test("valid hair color", () => {
      expect(solution.matchHairColor("hcl:#123abc")).toBe(true);
    });
    test("invalid hair color with wrong characters", () => {
      expect(solution.matchHairColor("hcl:#123abz")).toBe(false);
    });
    test("invalid hair color with missing #", () => {
      expect(solution.matchHairColor("hcl:123abc")).toBe(false);
    });
    test("invalid hair color with wrong length", () => {
      expect(solution.matchHairColor("hcl:#123ab")).toBe(false);
    });
  });

  // Eye Color Tests
  describe("matchEyeColor", () => {
    test("valid eye color", () => {
      expect(solution.matchEyeColor("ecl:brn")).toBe(true);
    });
    test("invalid eye color", () => {
      expect(solution.matchEyeColor("ecl:wat")).toBe(false);
    });
  });

  // Passport ID Tests
  describe("matchPassportID", () => {
    test("valid passport ID", () => {
      expect(solution.matchPassportID("pid:000000001")).toBe(true);
    });
    test("passport ID too long", () => {
      expect(solution.matchPassportID("pid:0123456789")).toBe(false);
    });
    test("passport ID too short", () => {
      expect(solution.matchPassportID("pid:12345678")).toBe(false);
    });
    test("passport ID with non-digit characters", () => {
      expect(solution.matchPassportID("pid:abcdefghi")).toBe(false);
    });
  });
});
