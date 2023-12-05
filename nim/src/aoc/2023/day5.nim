include "../prelude"
import strformat, unittest, algorithm, "../utils/seq_utils"

type AlmanacRange = tuple[destination: int, source: int, rangeLength: int]

type AlmanacMap = tuple [
    fromType: string, 
    toType: string, 
    maps: seq[AlmanacRange]
    ]

type SeedRange = tuple[start: int, length: int]

func parseMap(map: string): AlmanacMap =
    let x = map.split(":")
    let types = x[0].replace("map", "").strip.split("-to-")
    let grid = x[1].strip.splitLines

    var maps = newSeq[AlmanacRange]()
    for line in grid:
        let parts = line.strip.split(" ").mapIt(it.parseInt)
        maps.add((destination: parts[0], source: parts[1], rangeLength: parts[2]))
    
    (fromType: types[0], toType: types[1], maps: maps)

func parsePairs(seeds: seq[int]): seq[SeedRange] =
    for i in countup(0, seeds.len - 1, 2):
        result.add((start: seeds[i], length: seeds[i + 1]))

func createLookupTable(maps: seq[AlmanacMap]): Table[string, AlmanacMap] =
    for map in maps:
        result[map.fromType] = map

func getMappedValue(value: int, map: AlmanacMap): int =
    ## Any source numbers that aren't mapped correspond to the same destination number
    for r in map.maps:
        if value in r.source..r.source + r.rangeLength:
            return r.destination + value - r.source
    return value

func getMappedValue(value: int, lookupTable: Table[string, AlmanacMap]): int =
    result = value
    var map = lookupTable["seed"]
    while map.toType != "location":
        result = result.getMappedValue(map)
        map = lookupTable[map.toType]
    result = result.getMappedValue(map)

func getSliced(seed: SeedRange, almanac: AlmanacRange): tuple[left: SeedRange, middle: SeedRange, right: SeedRange] =
    let r = almanac.source..(almanac.source + almanac.rangeLength)
    let s = seed.start..(seed.start + seed.length)
    let invalidRange = (start: -1, length: -1)

    # ..[axxb]..
    if s.a in r and s.b in r:
        result = (left: invalidRange, middle: seed, right: invalidRange)
    # .a[xxx]b.
    elif s.a < r.a and s.b > r.b:
        result = (
            left: (start: s.a, length: r.a - s.a), 
            middle: (start: r.a, length: r.b - r.a), 
            right: (start: r.b, length: s.b - r.b))
    # .a[xxb].
    elif s.a < r.a and s.b in r:
        result = (
            left: (start: s.a, length: r.a - s.a), 
            middle: (start: r.a, length: s.b - r.a), 
            right: invalidRange)
    # .[axx]b.
    elif s.a in r and s.b > r.b:
        result = (
            left: invalidRange, 
            middle: (start: s.a, length: r.b - s.a), 
            right: (start: r.b, length: s.b - r.b))
    # .ab[...].
    elif s.b > r.a:
        result = (left: seed, middle: invalidRange, right: invalidRange)
    # .[...]ab.
    elif s.a < r.b:
        result = (left: invalidRange, middle: invalidRange, right: seed)
    else:
        raise newException(Exception, "Invalid slice")

    if result.left.length == 0:
        result.left = invalidRange
    if result.middle.length == 0:
        result.middle = invalidRange
    if result.right.length == 0:
        result.right = invalidRange
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
proc getMappedRange(map: AlmanacMap, value: SeedRange): seq[SeedRange] =
    ## Any source numbers that aren't mapped correspond to the same destination number
    result = @[]

    for r in map.maps:
        let sliced = getSliced(value, r)
        if(sliced.middle.start == -1): # skip un-mapped ranges
            continue

        if(sliced.left.start != -1):
            result = result.concat(getMappedRange(map, sliced.left))
        
        result.add((start:  r.destination + sliced.middle.start - r.source, length: sliced.middle.length))

        if(sliced.right.start != -1):
            result = result.concat(getMappedRange(map, sliced.right))

    if result.len == 0:
        return @[value]

proc getMappedRange(lookupTable: Table[string, AlmanacMap], value: SeedRange): seq[SeedRange] =
    result = @[value]
    var map = lookupTable["seed"]
    while map.toType != "location":
        result = result.mapIt(map.getMappedRange(it)).flatten().deduplicate
        map = lookupTable[map.toType]
    result = result.mapIt(map.getMappedRange(it)).flatten()

proc main() =
    let input = readInput(2023, 5, test = false).strip.split("\n\n")

    let seeds = input[0].replace("seeds: ", "").split(" ").mapIt(it.parseInt)
    let maps = input[1..^1].map(parseMap)
    let lookupTable = createLookupTable(maps)

    echo "Part 1: ", seeds.mapIt(it.getMappedValue(lookupTable)).min

    let seedPairs = parsePairs(seeds)

    echo "Part 2: ", seedPairs.mapIt(lookupTable.getMappedRange(it)).flatten().sortedByIt(it.start)[0].start

suite "Day 5":
    let invalidRange = (start: -1, length: -1)
    let almanac = (destination: 0, source: 2, rangeLength: 2) # 2..4

    test ".a[xxx]b.":
        let seed = (start: 1, length: 4) # 1..5
        check getSliced(seed, almanac) == 
            (left:   (start: 1, length: 1), 
            middle: (start: 2, length: 2), 
            right:  (start: 4, length: 1))
    test "..[axb]..":
        let seed = (start: 2, length: 2) # 2..4
        check getSliced(seed, almanac) == 
            (left:   invalidRange, 
            middle: (start: 2, length: 2), 
            right:  invalidRange)
    test "..[abx]..":
        let seed = (start: 2, length: 1) # 2..3
        check getSliced(seed, almanac) == 
            (left:   invalidRange, 
            middle: (start: 2, length: 1), 
            right:  invalidRange)
    test ".a[xxb]..":
        let seed = (start: 1, length: 3) # 1..4
        check getSliced(seed, almanac) == 
            (left:   (start: 1, length: 1), 
            middle: (start: 2, length: 2), 
            right:  invalidRange)
    test "..[axx]b.":
        let seed = (start: 2, length: 3) # 2..5
        check getSliced(seed, almanac) == 
            (left:   invalidRange, 
            middle: (start: 2, length: 2), 
            right:  (start: 4, length: 1))
    test "ab[...]..":
        let seed = (start: 0, length: 2) # 0..1
        check getSliced(seed, almanac) == 
            (left:   (start: 0, length: 2), 
            middle: invalidRange, 
            right:  invalidRange)
    test "..[...]ab":
        let seed = (start: 4, length: 2) # 4..5
        check getSliced(seed, almanac) == 
            (left:   invalidRange, 
            middle: invalidRange, 
            right:  (start: 4, length: 2))

main()
