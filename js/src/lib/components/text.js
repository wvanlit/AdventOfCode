import p5 from "p5";
import { bg } from "../canvas";

export default class Text {
  /**
   * @param {p5} p
   * @param {string} s - the text
   * @param {number} x
   * @param {number} y
   */
  constructor(p, s, x, y) {
    this.p = p;
    this.s = s;
    this.x = x;
    this.y = y;
  }

  set(s) {
    this.s = s.trim();
  }

  draw() {
    let textSize = 10;

    this.p.fill(bg);
    let w = (this.prevS ?? this.s).length * (textSize / 2);
    this.p.rect(this.x, this.y - textSize, w, textSize + 1);

    this.p.fill(255);
    this.p.textSize(10);
    this.p.text(this.s, this.x, this.y);

    // save as prev to erase it later
    this.prevS = this.s;
  }
}
