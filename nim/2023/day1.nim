include ../prelude

let input = readInput(2023, 1)
let lines = input.strip.splitLines.map(s => s.strip)

proc digits(lines: seq[string]): int =
  result = 0
  for line in lines:
    var first = -1
    var last = -1

    for ch in line:
      if ch.isDigit.not:
        continue

      let v = ch.int - '0'.int

      if first == -1:
        first = v
      else:
        last = v

    result += first * 10 + (if last == -1: first else: last)

proc deliteralString(line: string): string =
  return line
    .replace("one", "o1e")
    .replace("two", "t2o")
    .replace("three", "th3ee")
    .replace("four", "f4r")
    .replace("five", "f5e")
    .replace("six", "s6x")
    .replace("seven", "s7n")
    .replace("eight", "e8t")
    .replace("nine", "n9e")

echo "P1: ", digits(lines)
echo "P2: ", digits(lines.map(deliteralString))
