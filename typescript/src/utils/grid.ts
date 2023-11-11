export class Grid2D<T> {
  constructor(public arr: T[][]) {}

  getUnsafe(row: number, column: number) {
    return this.arr[row][column];
  }

  outOfBounds(row: number, column: number) {
    return row < 0 || row >= this.arr.length || column < 0 || column >= this.arr[row].length;
  }

  get(row: number, column: number): T | null {
    if (this.outOfBounds(row, column)) {
      return null;
    }
    return this.arr[row][column];
  }

  set(row: number, column: number, value: T): T | null {
    if (this.outOfBounds(row, column)) {
      return null;
    }

    this.arr[row][column] = value;

    return value;
  }

  getAllNeighbors(row: number, column: number): T[] {
    const neighbors: T[] = [];
    const directions = [-1, 0, 1];

    for (let i of directions) {
      for (let j of directions) {
        // Skip the center
        if (i === 0 && j === 0) continue;

        const neighborRow = row + i;
        const neighborColumn = column + j;

        if (!this.outOfBounds(neighborRow, neighborColumn)) {
          neighbors.push(this.arr[neighborRow][neighborColumn]);
        }
      }
    }

    return neighbors;
  }

  forEach(callback: (value: T, row: number, column: number) => void): void {
    for (let row = 0; row < this.arr.length; row++) {
      for (let column = 0; column < this.arr[row].length; column++) {
        callback(this.arr[row][column], row, column);
      }
    }
  }

  raycast(row: number, column: number, rowStep: number, colStep: number): T[] {
    const result: T[] = [];

    let currentRow = row + rowStep;
    let currentColumn = column + colStep;

    while (!this.outOfBounds(currentRow, currentColumn)) {
      result.push(this.arr[currentRow][currentColumn]);
      currentRow += rowStep;
      currentColumn += colStep;
    }

    return result;
  }

  raycastAllDirections(row: number, column: number) {
    const directions = [-1, 0, 1];

    const result: T[][] = [];

    for (let i of directions) {
      for (let j of directions) {
        // Skip the center
        if (i === 0 && j === 0) continue;

        result.push(this.raycast(row, column, i, j));
      }
    }

    return result;
  }

  print(str: (v: T) => string) {
    for (let row = 0; row < this.arr.length; row++) {
      let s = "";

      for (let column = 0; column < this.arr[row].length; column++) {
        s += str(this.arr[row][column]);
      }

      console.log(s);
    }
  }
}
