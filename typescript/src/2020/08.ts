import { Solution } from "../utils/solution";

type Instruction = { type: string; argument: number };

class Day extends Solution {
  constructor() {
    super(2020, 8);
  }

  parseInstruction(instruction: string): Instruction {
    const type = instruction.slice(0, 4).trim();
    const argument = parseInt(instruction.slice(4).trim());
    return { type, argument };
  }

  async part1(input: string): Promise<string> {
    const instructions = input.split("\n").map(this.parseInstruction);
    const executeSet = new Set<number>();

    let current = 0;
    let accumulator = 0;

    while (!executeSet.has(current)) {
      executeSet.add(current);

      const i = instructions[current];
      switch (i.type) {
        case "nop":
          current++;
          break;
        case "jmp":
          current += i.argument;
          break;
        case "acc":
          current++;
          accumulator += i.argument;
          break;
      }
    }

    this.Part1Bar.increment();

    return accumulator.toString();
  }

  simulate(instructions: Instruction[], maxCycles: number): number {
    let current = 0;
    let cycles = 0;
    let accumulator = 0;

    while (current < instructions.length && cycles < maxCycles) {
      cycles++;

      const i = instructions[current];
      switch (i.type) {
        case "jmp":
          current += i.argument;
          break;
        case "acc":
          current++;
          accumulator += i.argument;
          break;
        case "nop":
          current++;
          break;
      }
    }

    if (cycles >= maxCycles) return Number.NaN;
    return accumulator;
  }

  async part2(input: string): Promise<string> {
    const instructions = input.split("\n").map(this.parseInstruction);
    const MAX_CYCLES = 10_000;

    this.Part2Bar.setTotal(instructions.length);

    for (let index = 0; index < instructions.length; index++) {
      this.Part2Bar.increment();

      const element = instructions[index];

      // Only swap jmp/nop
      if (element.type === "acc") continue;

      instructions[index] = {
        type: element.type === "nop" ? "jmp" : "nop",
        argument: element.argument,
      };

      const solution = this.simulate(instructions, MAX_CYCLES);

      instructions[index] = element;

      if (!Number.isNaN(solution)) return solution.toString();
    }

    return "FAILED";
  }
}

if (import.meta.main) {
  new Day().execute(false);
}

export default Day;
