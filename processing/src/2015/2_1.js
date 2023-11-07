import p5 from "p5";
import { bg, setupCanvas, setupFrame } from "../lib/canvas";
import Graph from "../lib/components/graph";
import Text from "../lib/components/text";
import input from "./inputs/2.imba";
import { getRandomColor } from "../lib/components/random_color";

/**
 * @param {p5} p
 */
export default (p) => {
  const frameRate = 100;
  const presents = input
    .split("\n")
    .map((l) => l.split("x").map((n) => parseInt(n)))
    .map((p) => ({ l: p[0], w: p[1], h: p[2] }));

  const area = ({ l, w, h }) => {
    let sideLW = l * w;
    let sideWH = w * h;
    let sideHL = h * l;

    let smallest = Math.min(sideLW, sideWH, sideHL);

    return smallest + 2 * sideLW + 2 * sideWH + 2 * sideHL;
  };

  console.log(presents);

  let index = 0;
  let totalAreaUsed = 0;

  /** @type {Text} */
  let counter;
  /** @type {Text} */
  let tracker;

  let step = () => {
    if (index >= presents.length - 1) {
      return;
    }

    let present = presents[index++];
    totalAreaUsed += area(present);

    let sizeMultiplier = 2;
    p.rectMode("center");

    // Draw the present
    p.fill(getRandomColor());
    p.rect(
      p.width / 2,
      p.height / 2,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    // Draw the wrapping paper
    p.fill(getRandomColor());

    p.rect(
      p.width / 2 + present.l * sizeMultiplier,
      p.height / 2,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.rect(
      p.width / 2 - present.l * sizeMultiplier,
      p.height / 2,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.rect(
      p.width / 2,
      p.height / 2 - present.w * sizeMultiplier,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.rect(
      p.width / 2,
      p.height / 2 + present.w * sizeMultiplier,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.fill(getRandomColor());

    p.rect(
      p.width / 2 + present.l * sizeMultiplier,
      p.height / 2 + present.w * sizeMultiplier,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.rect(
      p.width / 2 - present.l * sizeMultiplier,
      p.height / 2 + present.w * sizeMultiplier,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.rect(
      p.width / 2 + present.l * sizeMultiplier,
      p.height / 2 - present.w * sizeMultiplier,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    p.rect(
      p.width / 2 - present.l * sizeMultiplier,
      p.height / 2 - present.w * sizeMultiplier,
      present.l * sizeMultiplier,
      present.w * sizeMultiplier
    );

    // Update text
    counter.set(`Present #${index}`);
    tracker.set(`Used ${totalAreaUsed} sqr feet of wrapping paper`);
  };

  p.setup = () => {
    setupCanvas(p);

    p.frameRate(frameRate);

    counter = new Text(p, "", 10, 20);
    tracker = new Text(p, "", 10, 40);

    step();
  };

  p.draw = () => {
    setupFrame(p);
    step();

    counter.draw();
    tracker.draw();
  };
};
