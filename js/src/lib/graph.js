import p5 from "p5";
import { worldToCanvas } from "./coordinate";

export class Graph {
    /**
     * @param {p5} p 
     * @param {number} x1 
     * @param {number} x2
     * @param {number} y1 
     * @param {number} y2 
     */
    constructor(p, x1, x2, y1, y2) {
        this.p = p;

        this.x1 = x1;
        this.x2 = x2;

        this.y1 = y1;
        this.y2 = y2;

        this.canvasMiddleY = (this.p.height / 2)
        this.canvasMiddleX = (this.p.width / 2)
    }

    drawGrid() {
        this.p.stroke(255);
        this.p.strokeWeight(1);

        let { cx: zX, cy: zY } = this.getPoint(0, 0);

        // Draw x-axis
        this.p.line(0, zY, this.p.width, zY);
        // Draw y-axis
        this.p.line(zX, 0, zX, this.p.height);

        // Write axis sizes
        this.p.fill(255);
        this.p.strokeWeight(0);
        // y-axis
        this.p.text(this.y2, zX + 5, 10);
        this.p.text(this.y1, zX + 5, this.p.height);
        // x-axis
        this.p.text(this.x1, 5, zY + 15);
        this.p.text(this.x2, this.p.width - (8 * (this.x2.toString().length)), zY + 15);
    }

    drawPoint(x, y, c) {
        let { cx, cy } = this.getPoint(x, y);

        this.p.fill(c ?? '#FFF');
        this.p.circle(cx, cy, 3);
    }

    getPoint(x, y) {
        return worldToCanvas(
            x, y,
            this.x1, this.x2,
            this.y1, this.y2,
            this.p.width, this.p.height);
    }
}