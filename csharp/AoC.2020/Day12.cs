using AoC.Shared;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Spectre.Console;
using Xunit.Abstractions;

namespace AoC._2020;

public class Day12 (ITestOutputHelper outputHelper) : SolutionBase(2020, 12, false, outputHelper)
{
    private record Action(string Type, int Value)
    {
        public static Action Parse(string s) => new(
            Type: s[..1],
            Value: int.Parse(s[1..]));
    }

    private record ShipState(
        int NorthSouth = 0,
        int EastWest = 0,
        Direction Direction = Direction.East
    );

    private static Direction Turn(Direction current, string dir, int degrees)
    {
        var steps = degrees / 90;
        var sign = dir == "L" ? -1 : 1;
        var index = (int)current;
        var outcome = (Direction)((4 + index + steps * sign) % 4);

        return outcome;
    }

    public override async Task<Answer> Part1(string input)
    {
        var actions = input.SplitLines().Select(Action.Parse).ToArray();
        var state = new ShipState();

        state = actions.Aggregate(state, (current, a) => a.Type switch
        {
            "N" => current with { NorthSouth = current.NorthSouth + a.Value },
            "S" => current with { NorthSouth = current.NorthSouth - a.Value },
            "E" => current with { EastWest = current.EastWest + a.Value },
            "W" => current with { EastWest = current.EastWest - a.Value },
            "L" => current with { Direction = Turn(current.Direction, "L", a.Value) },
            "R" => current with { Direction = Turn(current.Direction, "R", a.Value) },
            "F" => current.Direction switch
            {
                Direction.North => current with { NorthSouth = current.NorthSouth + a.Value },
                Direction.East => current with { NorthSouth = current.NorthSouth - a.Value },
                Direction.South => current with { EastWest = current.EastWest + a.Value },
                Direction.West => current with { EastWest = current.EastWest - a.Value },
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => throw new ArgumentOutOfRangeException()
        });

        return int.Abs(state.NorthSouth) + int.Abs(state.EastWest);
    }


    private record ShipWithWaypointState(
        int NorthSouth = 0,
        int EastWest = 0,
        int WaypointNorthSouthOffset = 1,
        int WaypointEastWestOffset = 10
    )
    {
        public int WaypointNorthSouth => NorthSouth + WaypointNorthSouthOffset;
        public int WaypointEastWest => EastWest + WaypointEastWestOffset;

        public ShipWithWaypointState Forward(int amount) =>
            this with
            {
                NorthSouth = NorthSouth + amount * WaypointNorthSouthOffset,
                EastWest = EastWest + amount * WaypointEastWestOffset,
            };

        public ShipWithWaypointState RotateWaypoint(string direction, int degrees)
        {
            var steps = degrees / 90;

            var ns = WaypointNorthSouthOffset;
            var ew = WaypointEastWestOffset;

            var (n, e) = (direction, steps) switch
            {
                ("L", 0) => (ns, ew),
                ("L", 1) => (ew, -ns),
                ("L", 2) => (-ns, -ew),
                ("L", 3) => (-ew, ns),

                ("R", 0) => (ns, ew),
                ("R", 1) => (-ew, ns),
                ("R", 2) => (-ns, -ew),
                ("R", 3) => (ew, -ns),

                _ => throw new ArgumentOutOfRangeException()
            };

            return this with
            {
                WaypointNorthSouthOffset = n,
                WaypointEastWestOffset = e,
            };
        }

        public override string ToString()
        {
            return
                $"Ship: [NS: {NorthSouth}, EW: {EastWest}], Waypoint: [{WaypointNorthSouth}, {WaypointEastWest}], Waypoint Offset [{WaypointNorthSouthOffset}, {WaypointEastWestOffset}]";
        }
    }

    public override async Task<Answer> Part2(string input)
    {
        var actions = input.SplitLines().Select(Action.Parse).ToArray();
        var state = new ShipWithWaypointState();

        foreach (var action in actions)
        {
            state = action.Type switch
            {
                // Update Waypoint
                "N" => state with { WaypointNorthSouthOffset = state.WaypointNorthSouthOffset + action.Value },
                "S" => state with { WaypointNorthSouthOffset = state.WaypointNorthSouthOffset - action.Value },
                "E" => state with { WaypointEastWestOffset = state.WaypointEastWestOffset + action.Value },
                "W" => state with { WaypointEastWestOffset = state.WaypointEastWestOffset - action.Value },
                // Turn Waypoint around Ship
                "L" => state.RotateWaypoint("L", action.Value),
                "R" => state.RotateWaypoint("R", action.Value),
                "F" => state.Forward(action.Value),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        AnsiConsole.WriteLine(state.ToString());

        return int.Abs(state.NorthSouth) + int.Abs(state.EastWest);
    }
}