import "../utils/io_utils"
import strutils, strformat, sequtils, math, tables, re

type Part = tuple[x: int, m: int, a: int, s: int]
type Rule = tuple[category: char, operator: char, target: int, destination: string]
type Workflow = tuple[id: string, rules: seq[Rule], otherwise: string]

func `$`(p: Part): string = fmt"x={p.x} m={p.m} a={p.a} s={p.s}"
func `$`(r: Rule): string = fmt"{r.category}{r.operator}{r.target}:{r.destination}"
func `$`(w: Workflow): string = 
    let rules = w.rules.map(`$`).join(", ")
    fmt"id={w.id} rules=[{rules}] else={w.otherwise}"

let partPattern = re"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}"

proc parsePart(line: string): Part =
    var matches: array[4, string]
    assert match(line, partPattern, matches)

    (x: matches[0].parseInt, 
     m: matches[1].parseInt, 
     a: matches[2].parseInt, 
     s: matches[3].parseInt)

func parseRule(line: string): Rule =
    # a>1716:R
    let sections = line.split(":")
    result.destination = sections[1].strip
    result.category = sections[0][0]
    result.operator = sections[0][1]
    result.target = sections[0][2..^1].parseInt

func parseWorkflow(line: string): Workflow =
    # px{a<2006:qkq,m>2090:A,rfg}
    let sections = line.split("{")
    result.id = sections[0].strip
    let rules = sections[1][0..^2].split(",")
    result.rules = rules[0..^2].map(parseRule)
    result.otherwise = rules[^1]

func workflowMap(section: string): Table[string, Workflow] =
    result = initTable[string, Workflow]()
    for line in section.splitLines:
        let workflow = parseWorkflow(line)
        result[workflow.id] = workflow

func processPart(part: Part, workflows: Table[string, Workflow]): char =
    var outcome = "in"

    while outcome != "R" and outcome != "A":
        let workflow = workflows[outcome]
        outcome = workflow.otherwise

        for r in workflow.rules:
            let value = case r.category:
                of 'x': part.x
                of 'm': part.m
                of 'a': part.a
                of 's': part.s
                else: raise newException(ValueError, "Unknown category: " & $r.category)

            let matches = case r.operator:
                of '<': value < r.target
                of '>': value > r.target
                else: raise newException(ValueError, "Unknown operator: " & $r.operator)

            if matches:
                outcome = r.destination
                break

    result = outcome[0]
    assert result == 'R' or result == 'A'

proc main() =
    let input = readInput(2023, 19, test=false).strip
    let sections = input.split("\n\n")

    let workflows = workflowMap(sections[0])
    let parts = sections[1].splitLines.map(parsePart)


    echo "Part 1: ", parts.filterIt(processPart(it, workflows) == 'A').mapIt(it.x + it.m + it.a + it.s).sum

main()