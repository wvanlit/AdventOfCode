import "../utils/io_utils"
import strutils, sequtils, tables, math, algorithm, sugar

type Card = char
type Hand = tuple[cards: seq[Card], scoreP1: int, scoreP2: int, bid: int]

const cardValues = {
        '2': "02", '3': "03", '4': "04", '5': "05",
        '6': "06", '7': "07", '8': "08", '9': "09",
        'T': "10", 'J': "11", 'Q': "12", 'K': "13", 'A': "14",
    }.toTable

func handScore(largest: int, second: int): string =
    case largest:
        of 5: "10"
        of 4: "09"
        of 3:
            if second == 2: "08" else: "07"
        of 2:
            if second == 2: "06" else: "05"
        else: "01"

func calculateScoreP1(cards: seq[Card]): int =
    let scoreStr = cards.mapIt(cardValues[it]).join("")
    let counts = cards.toCountTable.pairs.toSeq.sortedByIt(it[1]).reversed

    let largestGroup = counts[0][1]
    let secondLargestGroup = if largestGroup != 5: counts[1][1] else: 0

    let handTypeScore: string = handScore(largestGroup, secondLargestGroup)

    (handTypeScore & scoreStr).parseInt

func calculateScoreP2*(cards: seq[Card]): int =
    let scoreStr = cards.mapIt(cardValues[it]).mapIt(if it == "11": "01" else: it).join("")
    let count = cards.toCountTable
    let counts = count.pairs.toSeq.sortedByIt(it[1]).filterIt(it[0] != 'J').reversed

    let largestGroup = if count['J'] != 5: counts[0][1] else: 0
    let secondLargestGroup = if largestGroup != 5 and counts.len > 1: counts[1][1] else: 0

    let handTypeScore: string = handScore(largestGroup + count['J'], secondLargestGroup)

    (handTypeScore & scoreStr).parseInt

func parseCards(lines: seq[string]): seq[Hand] =
    for handStr in lines:
        let parts = handStr.split(" ")
        let cards = parts[0].toSeq
        result.add((cards: cards,
                    scoreP1: calculateScoreP1(cards),
                    scoreP2: calculateScoreP2(cards),
                    bid: parts[1].strip.parseInt))

func calculateWinnings(hands: seq[Hand]): int =
    var winnings: seq[int] = @[]

    for index in 0..<hands.len:
        winnings.add(hands[index].bid * (index + 1))

    winnings.sum

func part1(hands: seq[Hand]): int = calculateWinnings(hands.sortedByIt(it.scoreP1))
func part2(hands: seq[Hand]): int = calculateWinnings(hands.sortedByIt(it.scoreP2))

proc main() =
    let input = readInput(2023, 7, test = false).strip.splitLines
    let hands = parseCards(input)

    echo "Part 1: ", part1(hands)
    echo "Part 2: ", part2(hands)

main()
