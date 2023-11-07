import { loadFiles, loadSketch } from './lib/files'

loadFiles();

document.getElementById("load-sketch").onclick = loadSketch