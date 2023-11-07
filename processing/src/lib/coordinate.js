/**
 * Translates world coordinates to canvas coordinates.
 *
 * @param {number} x - The world x-coordinate.
 * @param {number} y - The world y-coordinate.
 * @param {number} x1 - The minimum bound of the world x-coordinate.
 * @param {number} x2 - The maximum bound of the world x-coordinate.
 * @param {number} y1 - The minimum bound of the world y-coordinate.
 * @param {number} y2 - The maximum bound of the world y-coordinate.
 * @param {number} cw - The canvas width.
 * @param {number} ch - The canvas height.
 * @returns {{cx: number, cy: number}}An object containing the canvas x (`cx`) and y (`cy`) coordinates.
 * @example
 * const {cx, cy} = worldToCanvas(50, 50, 0, 100, 0, 100, 500, 500);
 * console.log(cx, cy); // Outputs: 250, 250
 */
export function worldToCanvas(x, y, x1, x2, y1, y2, cw, ch) {
  let cx = ((x - x1) / (x2 - x1)) * cw;
  let cy = ch - ((y - y1) / (y2 - y1)) * ch;
  return { cx, cy };
}
