import { A } from "@mobily/ts-belt";
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 6);
  }

  answers(input: string) {
    const lines = input.split("\n");
    const answers = [];
    let current = [];

    for (const line of lines) {
      if (line.trim() === "") {
        answers.push(current);
        current = [];
      } else {
        current.push(line)
      }
    }

    answers.push(current);

    return answers;
  }

  async part1(input: string): Promise<string> { 
    const answers = this.answers(input);
    const uniqueAnswers = answers.map(a => A.uniq(a.join("").split("")))

    return uniqueAnswers.map(u => u.length).reduce((p,c) => p + c).toString(); 
  }

  async part2(input: string): Promise<string> { 
    const answers = this.answers(input);
    const collectiveAnswers = answers
      .map(group => group.map(a => a.split("")))
      .map(group => group.reduce((p, c) => A.intersection(p, c) as string[]))

    return collectiveAnswers.map(u => u.length).reduce((p,c) => p + c).toString(); 
  }
}

if (import.meta.main) {
    new Day().execute(true);
}

export default Day;