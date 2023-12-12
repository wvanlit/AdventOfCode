import "../utils/io_utils", "../utils/seq_utils"
import algorithm, sequtils, strutils, tables, math, sugar, threadpool
{.experimental: "parallel".}

type Spring* = enum Operational, Damaged, Unknown
type Record* = tuple[springs: seq[Spring], groupOfDamagedSprings: seq[int]]

func parseSpring(s: char): Spring =
    case s:
        of '?': Unknown
        of '#': Damaged
        of '.': Operational
        else: raise newException(ValueError, "Invalid spring")

func parseRecord*(line: string): Record =
    let parts = line.split(" ");
    let springs = parts[0].toSeq.map(parseSpring)
    let groups = parts[1].split(",").mapIt(it.parseInt)

    return (springs, groups)

func unfold*(record: Record): Record =
    let springs = record.springs
    let damaged = record.groupOfDamagedSprings

    var unfoldedSprings = springs
    var unfoldedDamaged = damaged

    for _ in 0..3:
        unfoldedSprings.add(Unknown)
        for s in springs:
            unfoldedSprings.add(s)

        for i in damaged:
            unfoldedDamaged.add(i)
    
    (unfoldedSprings, unfoldedDamaged)

func groupSprings(springs: seq[Spring]): seq[seq[Spring]] =
    result = @[]
    var currentGroup: seq[Spring] = @[]
    var currentType: Spring = Unknown
    
    for s in springs:
        if s == currentType:
            currentGroup.add(s)
        else:
            if currentGroup.len > 0:
                result.add(currentGroup)
                currentGroup = @[]
            currentType = s
            currentGroup.add(s)
    if currentGroup.len > 0:
        result.add(currentGroup)

func isValidRecord(record: Record): bool =
    assert not record.springs.anyIt(it == Unknown), "Unknown spring in record"

    let damaged = record.springs.groupSprings.filterIt(it[0] == Damaged).mapIt(it.len)

    if damaged.len != record.groupOfDamagedSprings.len:
        return false

    return damaged.zip(record.groupOfDamagedSprings).allIt(it[0] == it[1])

func couldBeValid(springs: seq[Spring], damaged: seq[int]): bool =
    let firstUnknown = springs.find(Unknown)
    let springs = springs[0..firstUnknown].groupSprings.filterIt(it[0] == Damaged).mapIt(it.len)

    if springs.len > damaged.len:
        return false
        
    for i in 0..springs.len-1:
        if springs[i] > damaged[i]:
            return false

    return true

func findValidArrangements*(record: Record): int =
    var flat = record.springs
    let damaged = record.groupOfDamagedSprings

    func backtrack(idx: int): int =
        if idx == flat.len:
            if isValidRecord((flat, damaged)):
                return 1
        elif not couldBeValid(flat, damaged):
            return 0
        else:
            if flat[idx] != Unknown:
                return backtrack(idx+1)

            for s in [Operational, Damaged]:
                flat[idx] = s
                result += backtrack(idx+1)

            flat[idx] = Unknown

    result = backtrack(0)

if isMainModule:
    let input = readInput(2023, 12, test=true).strip.splitLines.map(parseRecord)

    echo "Part1: ", input.map(findValidArrangements).sum
    # echo "Part2: ", input.map(unfold).map(findValidArrangements).sum
