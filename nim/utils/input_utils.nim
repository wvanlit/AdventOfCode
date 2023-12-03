import strformat

proc readInput(year: uint, day: uint, test = false): string =
    let test_ext = if test: ".test" else: ""
    return open(fmt"/mnt/Projects/AdventOfCode/inputs/{year}/{day}{test_ext}.txt").readAll

export readInput