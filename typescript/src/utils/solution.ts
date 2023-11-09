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

  async execute(quiet = false) {
    const input = await this.input();

    if (!quiet) this.Part1Bar.start(1, 0);
    const p1 = await this.part1(input);
    if (!quiet) this.Part1Bar.stop();

    console.log(`${this.year} ${this.day} => ${p1}`);

    if (!quiet) this.Part2Bar.start(1, 0);
    const p2 = await this.part2(input);
    if (!quiet) this.Part2Bar.stop();

    console.log(`${this.year} ${this.day} => ${p2}`);
  }

  async input() {
    const input = await Bun.file(`../inputs/${this.year}/${this.day}.txt`).text();

    return input.trim();
  }

  abstract part1(input: string): Promise<string>;
  abstract part2(input: string): Promise<string>;
}
