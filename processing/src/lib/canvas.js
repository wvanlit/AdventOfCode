export let bg = getComputedStyle(document.documentElement).getPropertyValue('--card-background-color');

/**
 * @param {p5} p 
 */
export function setupCanvas(p) {
    p.createCanvas(600, 400)
    p.strokeWeight(0)
    p.stroke("fff")
    p.background(bg)
}

/**
 * @param {p5} p 
 */
export function setupFrame(p) {
    p.background(bg)
}
