include ../prelude

let input = readInput(2023, 1)
let lines = input.strip.splitLines.map(s => s.strip)

proc digits(lines: seq[string]): int =
  result = 0
  for line in lines:
    let nums = line.filterIt(it.isDigit).mapIt(it.int - '0'.int)
    
    var first = nums[0]
    var last = if nums.len > 1: nums[^1] else: first
    
    result += first * 10 + last

let numberMap = {
  "one": "o1e",
  "two": "t2o",
  "three": "th3ee",
  "four": "f4r",
  "five": "f5e",
  "six": "s6x",
  "seven": "s7n",
  "eight": "e8t",
  "nine": "n9e"
}.toTable()

proc deliteralString(line: string): string =
  result = line
  for k, v in numberMap.pairs:
    result = result.replace(k, v)

echo "P1: ", digits(lines)
echo "P2: ", digits(lines.map(deliteralString))
