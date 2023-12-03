include ../prelude
import npeg, math

type Round = tuple[r: int, g: int, b: int]
type Game = tuple[id: int, rounds: seq[Round]]

func parseRound(cubes: Table[string, int]): Round =
    return (
        r: cubes.getOrDefault("red", 0),
        g: cubes.getOrDefault("green", 0),
        b: cubes.getOrDefault("blue", 0)
    )

proc parse(input: string): seq[Game] =
    var games: seq[Game] = @[]
    var rounds: seq[Round] = @[]
    var cubes: Table[string, int] = initTable[string, int]()

    let parser = peg "games":
        games <- +(game * "\n")
        game <- "Game " * >+Digit * ": " * +round:
            games.add((id: parseInt($1), rounds: rounds))
            rounds = @[]
        round <- +(cube * ?", ") * ?"; ":
            rounds.add(parseRound(cubes))
            cubes.clear()
        cube <- (>+Digit * " " * >("red" | "green" | "blue")):
            cubes[$2] = parseInt($1)

    let _ = parser.match(input)

    return games

func part1(games: seq[Game]): int =
    result = games
        .filterIt(it.rounds.allIt(it.r <= 12 and it.g <= 13 and it.b <= 14))
        .mapIt(it.id)
        .sum

func part2(games: seq[Game]): int =
    result = games
            .mapIt(it.rounds.foldl((
            r: max(a.r, b.r),
            g: max(a.g, b.g),
            b: max(a.b, b.b))))
        .mapIt(it.r * it.g * it.b)
        .sum

let input = readInput(2023, 2, test = false).strip
let games = parse(input)

echo "Part 1: ", part1(games)
echo "Part 2: ", part2(games)


