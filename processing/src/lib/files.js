import p5 from "p5";

const modules = import.meta.glob("/src/**/*.js");
const yearDropdown = document.getElementById("year");
const dayDropdown = document.getElementById("day");
const partDropdown = document.getElementById("part");
const sketchContainer = document.getElementById("sketch");

yearDropdown.addEventListener("change", () => {
  localStorage.setItem("selectedYear", yearDropdown.value);
});

dayDropdown.addEventListener("change", () => {
  localStorage.setItem("selectedDay", dayDropdown.value);
});

partDropdown.addEventListener("change", () => {
  localStorage.setItem("selectedPart", partDropdown.value);
});

export function loadFiles() {
  const pathPattern = /^\/src\/(\d+)\/(\d+)_(\d+)\.js$/;

  for (const path in modules) {
    const match = pathPattern.exec(path);
    if (match) {
      const [, year, day, part] = match;
      if (!yearDropdown.querySelector(`option[value="${year}"]`)) {
        yearDropdown.add(new Option(year, year));
      }
      if (!dayDropdown.querySelector(`option[value="${day}"]`)) {
        dayDropdown.add(new Option(`Day ${day}`, day));
      }
      if (!partDropdown.querySelector(`option[value="${part}"]`)) {
        partDropdown.add(new Option(`Part ${part}`, part));
      }
    }
  }

  const cachedYear = localStorage.getItem("selectedYear");
  if (cachedYear) {
    yearDropdown.value = cachedYear;
  }

  const cachedDay = localStorage.getItem("selectedDay");
  if (cachedDay) {
    dayDropdown.value = cachedDay;
  }

  const cachedPart = localStorage.getItem("selectedPart");
  if (cachedPart) {
    partDropdown.value = cachedPart;
  }

  if (cachedYear && cachedDay && cachedPart) {
    loadSketch();
  }
}

export async function loadSketch() {
  // Clear existing sketch
  if (window.currentSketch) {
    window.currentSketch.remove();
  }

  const year = yearDropdown.value;
  const day = dayDropdown.value;
  const part = partDropdown.value;

  const modulePath = `/src/${year}/${day}_${part}.js`;

  try {
    if (modules[modulePath]) {
      const module = await modules[modulePath]();
      const sketchFunction = module.default;

      window.currentSketch = new p5(sketchFunction, sketchContainer);
    } else {
      console.error("Module not found:", modulePath);
    }
  } catch (err) {
    console.error("Failed to load module:", err);
  }

  localStorage.setItem("selectedYear", year);
  localStorage.setItem("selectedDay", day);
  localStorage.setItem("selectedPart", part);
}
