include "../prelude"
import math, "../utils/table_utils"

type Card = tuple[id: int, winning: seq[int], numbers: seq[int]]
type CardCount = CountTable[int]

func parse(line: string): Card =
    let ticket = line.split(":")
    let parts = ticket[1].split("|")
    let id = ticket[0].replace("Card", "").strip.parseInt
    let winning = parts[0].strip.split(" ").filterIt(
            not it.isEmptyOrWhitespace).mapIt(it.parseInt)
    let numbers = parts[1].strip.split(" ").filterIt(
            not it.isEmptyOrWhitespace).mapIt(it.parseInt)
    (id, winning, numbers)

func hasMatchingNumbers(card: Card): int = 
    card.numbers.filterIt(card.winning.contains(it)).len

func calcPoints(card: Card): int =
    let hits = hasMatchingNumbers(card)
    if hits == 0: 0 else: 2 ^ (hits - 1)

func calculateResultingCards(card: Card, cards: seq[Card], cache: TableRef[int, CardCount]): CardCount =
    result = toCountTable[int]([card.id])

    if cache.hasKey(card.id): return cache[card.id]

    let next = (1..hasMatchingNumbers(card)).toSeq
        .mapIt(card.id + it)
        .mapIt(calculateResultingCards(cards[it - 1], cards, cache))

    result = mergeInto(result, next)
    cache[card.id] = result

proc calculateWonCards(cards: seq[Card]):CardCount =
    var cache = newTable[int, CardCount]()
    result = result.mergeInto(cards.mapIt(calculateResultingCards(it, cards, cache)))

let cards = readInput(2023, 4, test = false).strip.splitLines.mapIt(parse(it))

echo "Part 1: ", cards.mapIt(calcPoints(it)).sum
echo "Part 2: ", calculateWonCards(cards).pairs.toSeq.mapIt(it[1]).sum
