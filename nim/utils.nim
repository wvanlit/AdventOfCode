import strformat

proc readInput(year: uint, day: uint): string =
    return open(fmt"/mnt/Projects/AdventOfCode/inputs/{year}/{day}.txt").readAll

export readInput