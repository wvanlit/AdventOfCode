import { Presets, SingleBar } from "cli-progress";

export abstract class Solution {
  public Part1Bar: SingleBar;
  public Part2Bar: SingleBar;

  constructor(public year: number, public day: number) {
    this.Part1Bar = new SingleBar({
      format: "#1 {bar} {value}/{total} ({percentage}%) [{duration}s/{eta}s]",
      barsize: 30,
      barCompleteChar: "▰",
      barIncompleteChar: "▱",
    });
    this.Part2Bar = new SingleBar({
      format: "#2 {bar} {value}/{total} ({percentage}%) [{duration}s/{eta}s]",
      barsize: 30,
      barCompleteChar: "▰",
      barIncompleteChar: "▱",
    });
  }

  async execute() {
    const input = await this.input();

    this.Part1Bar.start(1, 0);
    const p1 = await this.part1(input);
    this.Part1Bar.stop();

    console.log(`${this.year} ${this.day} => ${p1}`);

    this.Part2Bar.start(1, 0);
    const p2 = await this.part2(input);
    this.Part2Bar.stop();

    console.log(`${this.year} ${this.day} => ${p2}`);
  }

  input() {
    return Bun.file(`../inputs/${this.year}/${this.day}.txt`).text();
  }

  abstract part1(input: string): Promise<string>;
  abstract part2(input: string): Promise<string>;
}
