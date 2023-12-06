import "../utils/io_utils"
import strutils, sequtils, sugar, math

type Race = tuple[time: int, distance: int]

func parseMultiple(input: string): seq[Race] =
    let td = input.splitLines()
    let parseLine = (line: string) => line.split(":")[1].split(" ").filterIt(not it.isEmptyOrWhitespace).mapIt(it.parseInt())
    result = zip(parseLine(td[0]), parseLine(td[1]))

func parseSingle(input: string): Race =
    let td = input.splitLines()
    let parseLine = (line: string) => line.split(":")[1].split(" ").filterIt(not it.isEmptyOrWhitespace).join("").parseInt
    result = (parseLine(td[0]), parseLine(td[1]))

func runRace(race: Race, holdButton: int): tuple[won: bool, distance: int] =
    let remaining = race.time - holdButton
    result.distance = remaining * holdButton
    result.won = race.distance < result.distance

func winningCombinations(race: Race): int =
    for i in 0..race.time:
        let (won, dist) = race.runRace(i)
        if won: result += 1    

proc main() =
    let input = readInput(2023, 6, test=false)
    echo "Part 1: ", parseMultiple(input).mapIt(winningCombinations(it)).prod
    echo "Part 2: ", parseSingle(input).winningCombinations()

if isMainModule:
    main()