import { A } from "@mobily/ts-belt";
import { Solution } from "../utils/solution";

class Day extends Solution {
  constructor() {
    super(2020, 7);
  }

  parseBag(bag: string) {
    const match = bag.match(/(?<container>\w+ \w+) bags contain(?<contains>(\W\d+ (\w+) (\w+) bags?,?)+| no other bags)\./);
    const groups = match?.groups!;

    const containsBags = groups.contains === " no other bags" ? [] : groups.contains.split(",");

    return {
      color: groups.container,
      contains: containsBags
        .map((b) => b.replace(/bags?/, "").trim())
        .map((b) => {
          const match = b.match(/(?<amount>\d+) (?<color>.+)/);
          return { amount: parseInt(match?.groups?.amount!), color: match?.groups?.color! };
        }),
    };
  }

  /**
   * Recursive DFS to find target
   */
  containsBag(target: string, bag: string, lookup: Record<string, string[]>) {
    const contains = lookup[bag]!;

    if (contains.includes(target)) return true;

    for (const containedBag of contains) {
      if (this.containsBag(target, containedBag, lookup)) {
        return true;
      }
    }

    return false;
  }

  requiresBags(bag: string, lookup: Record<string, { color: string; amount: number }[]>) {
    const contains = lookup[bag]!;

    this.Part2Bar.setTotal(this.Part2Bar.getTotal() + 1);
    this.Part2Bar.increment();

    let total = 1;

    for (const containedBag of contains) {
      total += this.requiresBags(containedBag.color, lookup) * containedBag.amount;
    }

    return total;
  }

  async part1(input: string): Promise<string> {
    const lines = input.split("\n");
    const bags = lines.map(this.parseBag);

    const lookup: Record<string, string[]> = {};

    for (const bag of bags) {
      lookup[bag.color] = bag.contains.map((b) => b.color);
    }

    const colors = [];
    const target = "shiny gold";

    this.Part1Bar.setTotal(bags.length);

    for (const bag in lookup) {
      this.Part1Bar.increment();
      if (bag === target) continue; // Skip target

      if (this.containsBag(target, bag, lookup)) {
        colors.push(bag);
      }
    }

    return colors.length.toString();
  }

  async part2(input: string): Promise<string> {
    const lines = input.split("\n");
    const bags = lines.map(this.parseBag);

    const lookup: Record<string, { color: string; amount: number }[]> = {};

    for (const bag of bags) {
      lookup[bag.color] = bag.contains;
    }

    const target = "shiny gold";

    this.Part2Bar.setTotal(0);

    return (this.requiresBags(target, lookup) - 1).toString();
  }
}

if (import.meta.main) {
  new Day().execute(false);
}

export default Day;
