import "../utils/io_utils"
import sequtils, strutils, tables, math

type Spring* = enum Operational, Damaged, Unknown
type Record* = tuple[springs: seq[Spring], blocks: seq[int]]

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
    var unfoldedSprings = record.springs
    var unfoldedBlocks = record.blocks

    for _ in 0..3:
        unfoldedSprings.add(Unknown)
        for s in record.springs:
            unfoldedSprings.add(s)

        for i in record.blocks:
            unfoldedBlocks.add(i)
    
    (unfoldedSprings, unfoldedBlocks)

type SearchCache = TableRef[tuple[pi: int, bi: int, currentBlockLength: int], int]

func backtrack(
    pattern: seq[Spring], 
    blocks: seq[int], 
    pi: int,  # current position in the pattern
    bi: int, # current position in the blocks sequence
    currentBlockLength: int, # length of the current block of damaged springs
    memo: SearchCache): int =
    let state = (pi, bi, currentBlockLength)
    if state in memo:
        return memo[state]

    # Done!
    if pi == pattern.len:
        # Used all the blocks springs
        if bi == blocks.len and currentBlockLength == 0:
            return 1
        # Used all the blocks springs, but didn't end on a block boundary
        elif bi == blocks.len - 1 and currentBlockLength == blocks[bi]:
            return 1
        else:
            return 0

    # Operational Springs are only valid at the start or end of a block
    if pattern[pi] == Operational or pattern[pi] == Unknown:
        # start a new block
        if currentBlockLength == 0: 
            result += backtrack(pattern, blocks, pi+1, bi, 0, memo)
        # end the current block
        elif currentBlockLength > 0 and 
             bi < blocks.len     and 
             currentBlockLength == blocks[bi]: 
            result += backtrack(pattern, blocks, pi+1, bi+1, 0, memo)
        # All other states containing an Operational Spring are invalid
    # Otherwise, we must continue the current block
    if pattern[pi] == Damaged or pattern[pi] == Unknown: 
        result += backtrack(pattern, blocks, pi+1, bi, currentBlockLength+1, memo)

    memo[state] = result

func findValidArrangements*(record: Record): int =
    return backtrack(record.springs, record.blocks, 0, 0, 0, SearchCache())

if isMainModule:
    let input = readInput(2023, 12, test=false).strip.splitLines.map(parseRecord)

    echo "Part1: ", input.map(findValidArrangements).sum
    echo "Part2: ", input.map(unfold).map(findValidArrangements).sum
