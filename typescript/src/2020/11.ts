import { Grid2D } from "../utils/grid";
import { Solution } from "../utils/solution";

const EMPTY_SEAT = 0;
const OCCUPIED_SEAT = 1;
const NO_SEAT = null;

class Day extends Solution {
  constructor() {
    super(2020, 11);
  }

  /*
   * Notes:
   * floor (.), empty seat (L), occupied seat (#)
   * NULLL      0               1
   * Celular automata!
   */

  async part1(input: string): Promise<string> {
    const seats = input.split("\n").map((s) => s.split("").map((c) => (c === "." ? NO_SEAT : EMPTY_SEAT)));

    const layout = new Grid2D<number | null>(seats);

    type UpdatePosition = { row: number; column: number; value: number };
    const updates: UpdatePosition[] = [];

    function checkSeat(value: number | null, row: number, column: number) {
      if (value === NO_SEAT) return;

      const neighbors = layout.getAllNeighbors(row, column);
      const totalFilled = neighbors.reduce((p: number, c) => p + (c ?? 0), 0);

      if (value === EMPTY_SEAT && totalFilled === 0) {
        updates.push({ row, column, value: OCCUPIED_SEAT });
      } else if (value === OCCUPIED_SEAT && totalFilled >= 4) {
        updates.push({ row, column, value: EMPTY_SEAT });
      }
    }

    do {
      while (updates.length) {
        const u = updates.pop()!;
        layout.set(u.row, u.column, u.value);
      }

      layout.forEach(checkSeat);
    } while (updates.length);

    let filledSeats = 0;
    layout.forEach((v, r, c) => (filledSeats += v ?? 0));

    return filledSeats.toString();
  }

  async part2(input: string): Promise<string> {
    const seats = input.split("\n").map((s) => s.split("").map((c) => (c === "." ? NO_SEAT : c === "#" ? OCCUPIED_SEAT : EMPTY_SEAT)));
    const layout = new Grid2D<number | null>(seats);

    type UpdatePosition = { row: number; column: number; value: number };
    const updates: UpdatePosition[] = [];

    function checkSeat(value: number | null, row: number, column: number) {
      if (value === NO_SEAT) return;

      const neighbors = layout.raycastAllDirections(row, column).map((n) => n.filter((c) => c !== null)[0] ?? EMPTY_SEAT);
      const totalFilled = neighbors.reduce((p: number, c) => p + c, 0);

      if (value === EMPTY_SEAT && totalFilled === 0) {
        updates.push({ row, column, value: OCCUPIED_SEAT });
      } else if (value === OCCUPIED_SEAT && totalFilled >= 5) {
        updates.push({ row, column, value: EMPTY_SEAT });
      }
    }

    this.Part2Bar.setTotal(1);

    do {
      this.Part2Bar.setTotal(this.Part2Bar.getTotal() + 1);
      this.Part2Bar.increment();

      while (updates.length) {
        const u = updates.pop()!;
        layout.set(u.row, u.column, u.value);
      }

      layout.forEach(checkSeat);
    } while (updates.length);

    let filledSeats = 0;
    layout.forEach((v, r, c) => (filledSeats += v ?? 0));

    return filledSeats.toString();
  }
}

if (import.meta.main) {
  new Day().execute(false);
}

export default Day;
