# This is just an example to get you started. You may wish to put all of your
# tests into a single file, or separate them into multiple `test1`, `test2`
# etc. files (better names are recommended, just make sure the name starts with
# the letter 't').
#
# To run these tests, simply execute `nimble test`.

import unittest
import "aoc/2023/day5"
import "aoc/utils/io_utils"

suite "2023 - day 5":
    let invalidRange = (start: -1, length: -1)
    let almanac = (destination: 0, source: 2, rangeLength: 2) # 2..4
    let example = readInput(2023, 5, test = true)
    let input = readInput(2023, 5, test = false)

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

    test "Part 1 - Example Input":
        check part1(example) == 35

    test "Part 1 - Actual Input":
        check part1(input) == 240320250

    test "Part 2 - Example Input":
        check part2(example) == 46

    test "Part 2 - Actual Input":
        check part2(input) == 28580589

    