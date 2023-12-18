import "../utils/io_utils"
import strutils, sequtils, math, tables

func hashOperation(input: string): uint =
    var h: uint = 0
    for c in input:
        h = h + uint(c)
        h = h * 17
        h = h mod 256
    return h

type Lens = tuple[label: string, focal: uint]

func calculateFocusingPower(input: seq[string]): uint =
    result = 0
    var boxes : seq[seq[Lens]] = newSeq[seq[Lens]](256)

    for operation in input:

        if operation.contains("-"):
            let label = operation.split("-")[0]
            let index = hashOperation(label)
            boxes[index] = boxes[index].filterIt(it.label != label)
        elif operation.contains("="):
            let label = operation.split("=")[0]
            let focal = operation.split("=")[1].parseUInt()
            let index = hashOperation(label)
            let lens = (label, focal)

            var found = false
            for i, l in boxes[index]:
                if l.label == label:
                    found = true
                    boxes[index][i] = lens

            if not found:
                boxes[index].add(lens)
        else:
            raise newException(Exception, "Invalid operations: " & operation)
    
    
    var dupes = initCountTable[string]()

    for i, box in boxes:
        if box.len == 0:
            continue

        debugEcho "Box ", ($i).alignLeft(3), ": [", box.mapIt(it.label).join(", "), "]"
        for slot, lens in box:
            dupes.inc(lens.label)

            let boxIndex: uint = uint(i) + 1
            let slotIndex: uint = uint(slot) + 1

            let focusPower = boxIndex * slotIndex * lens.focal

            debugEcho lens.label.alignLeft(7), ": ", ($boxIndex).alignLeft(3), " * ", ($slotIndex).alignLeft(3), " * ", ($lens.focal).alignLeft(3), " = ", ($focusPower).align(5)
            result += focusPower


proc main() =
    let input = readInput(2023, 15, test=false).strip.split(",")
    
    echo "Part 1: ", input.map(hashOperation).sum
    echo "Part 2: ", calculateFocusingPower(input)

main()