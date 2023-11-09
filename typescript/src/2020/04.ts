import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 4);
  }

  containsField(passport: string, field: string) {
    return passport.includes(field + ":");
  }

  passports(input: string) {
    const lines = input.split("\n");
    const passports = [];
    let current = "";

    for (const line of lines) {
      if (line.trim() === "") {
        passports.push(current.trim());
        current = "";
      } else {
        current += " " + line;
      }
    }

    passports.push(current);

    return passports;
  }

  async part1(input: string): Promise<string> {
    const passports = this.passports(input);
    const required = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];

    this.Part1Bar.setTotal(passports.length);

    let valid = 0;

    for (const passport of passports) {
      const matches = required.map((field) => this.containsField(passport, field));
      const all = matches.every((m) => m === true);

      if (all) {
        valid++;
      }

      this.Part1Bar.increment();
    }

    return valid.toString();
  }

  validate(passport: string) {
    return (
      this.matchBirthYear(passport) &&
      this.matchIssueYear(passport) &&
      this.matchExpirationYear(passport) &&
      this.matchHeight(passport) &&
      this.matchHairColor(passport) &&
      this.matchEyeColor(passport) &&
      this.matchPassportID(passport)
    );
  }

  matchYear(passport: string, field: string, min: number, max: number) {
    const match = passport.match(`${field}:(\\d{4})(?= |$)`);
    const year = match ? parseInt(match[1]) : null;
    return year !== null && year >= min && year <= max;
  }

  matchBirthYear(passport: string) {
    return this.matchYear(passport, "byr", 1920, 2002);
  }

  matchIssueYear(passport: string) {
    return this.matchYear(passport, "iyr", 2010, 2020);
  }

  matchExpirationYear(passport: string) {
    return this.matchYear(passport, "eyr", 2020, 2030);
  }

  matchHeight(passport: string) {
    const match = passport.match(/hgt:(\d+)(cm|in)(?= |$)/);
    if (!match) return false;

    const height = parseInt(match[1]);
    const unit = match[2];

    if (unit === "cm") return height >= 150 && height <= 193;
    if (unit === "in") return height >= 59 && height <= 76;

    return false;
  }

  matchHairColor(passport: string) {
    return /hcl:#([0-9a-f]{6})(?= |$)/.test(passport);
  }

  matchEyeColor(passport: string) {
    return /ecl:(amb|blu|brn|gry|grn|hzl|oth)(?= |$)/.test(passport);
  }

  matchPassportID(passport: string) {
    return /pid:([0-9]{9})(?= |$)/.test(passport);
  }

  async part2(input: string): Promise<string> {
    const passports = this.passports(input);
    this.Part2Bar.setTotal(passports.length);

    let valid = 0;

    for (const passport of passports) {
      if (this.validate(passport)) {
        valid++;
      }

      this.Part2Bar.increment();
    }

    return valid.toString();
  }
}

if (import.meta.main) {
  new Day().execute();
}

export default Day;
