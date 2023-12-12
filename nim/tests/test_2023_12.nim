import unittest
import "aoc/2023/day12"
import "aoc/utils/io_utils"

suite "2023 - day 12":
    test "single solution sequence":
        check parseRecord("???.### 1,1,3").findValidArrangements() == 1

    test "small solution sequence":
        check parseRecord(".??..??...?##. 1,1,3").findValidArrangements() == 4

    test "medium solution sequence":
        check parseRecord("?????#???????#???#. 6,1,1,3").findValidArrangements() == 15

    test "unfolded sequence":
        check parseRecord(".??..??...?##. 1,1,3").unfold().findValidArrangements() == 16384