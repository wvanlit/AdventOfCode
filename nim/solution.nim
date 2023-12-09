import os, times, osproc

## 
## Runs and times a solution for a given year and day
## Also runs tests if they exist
## Fairly hacky, but it works
## 

proc runCommand(command: string): Duration =
    let start = now()
    discard execCmd(command)
    return now() - start

proc main() =
    if paramCount() < 2:
        echo "Usage: nim r solution.nim <year> <day>"
        echo paramCount()
        quit(1)

    let
        year = paramStr(1)
        day = paramStr(2)
        filePath = "src/aoc/" & year & "/day" & day & ".nim"
        testPath = "tests/test_" & year & "_" & day & ".nim"
        executablePath = "src/aoc/" & year & "/day" & day

    if fileExists(filePath):
        let compileTime = runCommand("nim c " & filePath)
        
        echo "Running " & year & " day " & day & "...\n==="
        let executionTime = runCommand(executablePath)

        echo "===\nCompile duration: " & $compileTime
        echo "Execution duration: " & $executionTime

        if fileExists(testPath):
            echo "===\n"
            let executionTime = runCommand("nim c -r " & testPath)

        # Clean up
        discard execCmd("rm " & executablePath)
    else:
        echo "File not found: " & filePath

main()
