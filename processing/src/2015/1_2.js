import p5 from "p5";
import { setupCanvas } from "../lib/canvas";
import Graph from "../lib/components/graph";
import Text from "../lib/components/text";
import input from "./inputs/1.txt?raw";

/**
 * @param {p5} p
 */
export default (p) => {
  const directions = input.split("").map((c) => (c === "(" ? 1 : -1));

  const stepsPerFrame = 5;
  const frameRate = 100;

  let curr = 0;
  let index = 0;

  /** @type {Graph} */
  let graph;
  /** @type {Text} */
  let counter;
  /** @type {Text} */
  let solution;

  let step = () => {
    if (curr < 0) {
      solution.set(`-1 @ ${index}`);
      solution.draw();
      p.noLoop();
    } else if (index < directions.length) {
      curr += directions[index++];
    } else {
      solution.set(`Failed to find solution`);
      solution.draw();
      p.noLoop();
    }
  };

  p.setup = () => {
    setupCanvas(p);

    p.frameRate(frameRate);

    graph = new Graph(
      p,
      0,
      directions.length, // x
      -700,
      300 //y
    );

    graph.drawGrid();

    counter = new Text(p, "", p.width / 2, 10);
    solution = new Text(p, "", p.width / 2, p.height / 2 - 50);
  };

  p.draw = () => {
    for (let index = 0; index < stepsPerFrame; index++) {
      step();
    }

    counter.set(`steps: ${index}, floor: ${curr}`);
    counter.draw();

    graph.drawPoint(index, curr, "#f87171", 3);
  };
};
