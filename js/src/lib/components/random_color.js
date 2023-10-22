export function getRandomColor() {
  const colors = ["#fff", "#aaa", "#f00", "#0f0", "#00f"];

  const index = Math.floor(Math.random() * colors.length);
  return colors[index];
}
