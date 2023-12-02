include ../prelude

let input = readInput(2023, 2)
let lines = input.strip.splitLines.map(s => s.strip)

type
    Round = tuple[r: int, g: int, b: int]
    Game = object
        id: int
        rounds: seq[Round]

proc parseGame(line: string): Game =
    ## Example input:
    ## Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
    result = Game(id: 0, rounds: newSeq[Round](0))

    let parts = line.split(":")

    result.id = parts[0].replace("Game", "").strip.parseInt

    for cubeset in parts[1].strip.split(";"):
        var r: Round = (r: 0, g: 0, b: 0)
        for cube in cubeset.split(",").mapIt(it.strip.split(" ")):
            if cube[1] == "red":
                r.r = cube[0].parseInt
            elif cube[1] == "green":
                r.g = cube[0].parseInt
            elif cube[1] == "blue":
                r.b = cube[0].parseInt
            else:
                raiseAssert("Unknown color: '" & ($cube) & "' on " & ($line))
        result.rounds.add(r)


let games = lines.map(parseGame)

let p1 = games
    .filterIt(it.rounds.allIt(it.r <= 12 and it.g <= 13 and it.b <= 14))
    .mapIt(it.id)
    .foldl(a + b)

echo "Part 1: " & $p1

let p2 = games
    .mapIt(it.rounds.
        foldl((
        r: max(a.r, b.r),
        g: max(a.g, b.g),
        b: max(a.b, b.b))))
    .mapIt(it.r * it.g * it.b)
    .foldl(a + b)

echo "Part 2: " & $p2
