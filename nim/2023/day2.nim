include ../prelude
import math

type
    Color = enum red, green, blue
    Round = tuple[r: int, g: int, b: int]
    Game = object
        id: int
        rounds: seq[Round]

proc parseGame(line: string): Game =
    ## Example input: "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
    let parts = line.split(":")

    result = Game(
        id: parts[0].replace("Game", "").strip.parseInt,
        rounds: newSeq[Round](0))

    let rounds =
        collect(newSeq):
            for cubeset in parts[1].strip.split(";"):
                var r: Round
                for cube in cubeset.split(","):
                    let c = cube.strip.splitWhitespace
                    let amount = c[0].parseInt

                    case parseEnum[Color](c[1].strip)
                        of Color.red: r.r = amount
                        of Color.green: r.g = amount
                        of Color.blue: r.b = amount
                r

    result.rounds = rounds

let lines = readInput(2023, 2).strip.splitLines.map(s => s.strip)
let games = lines.map(parseGame)

let p1 = games
    .filterIt(it.rounds.allIt(it.r <= 12 and it.g <= 13 and it.b <= 14))
    .mapIt(it.id)
    .sum

echo "Part 1: ", p1

let p2 = games
    .mapIt(it.rounds.foldl((
        r: max(a.r, b.r),
        g: max(a.g, b.g),
        b: max(a.b, b.b))))
    .mapIt(it.r * it.g * it.b)
    .sum

echo "Part 2: ", p2
