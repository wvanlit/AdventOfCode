import p5 from "p5"
import input from './inputs/1.txt?raw'
import { bg, setupCanvas, setupFrame } from "../lib/canvas"
import { Graph } from "../lib/graph"

/**
 * @param {p5} p 
 */
export default (p) => {
  const directions = input
    .split('')
    .map(c => c === '(' ? 1 : -1);

  let curr = 0;
  let index = 0;
  let graph;

  let step = () => {
    if (index < directions.length) {
      curr += directions[index++]
    } else {
      p.strokeWeight(0);
      p.fill("#fff");
      p.text(curr, p.width / 2, p.height / 2 - 50)
      p.noLoop()
    }
  }

  let baseX;
  let baseY;
  let boundsY;
  let stepModX;
  let stepModY;

  p.setup = () => {
    setupCanvas(p);
    p.frameRate(100);

    graph = new Graph(
      p,
      0, directions.length, // x
      -700, 300, //y
    )

    graph.drawGrid();
  }

  p.draw = () => {
    for (let index = 0; index < 5; index++) {
      step()
    }

    p.strokeWeight(0);
    p.fill(bg)
    p.rect(p.width / 2, 0, 160, 25)

    p.fill(255);
    p.text("Floor: " + curr, p.width / 2, 10)
    p.text("Step: " + index, p.width / 2 + 80, 10)

    graph.drawPoint(index, curr, "#f87171");
  }
} 