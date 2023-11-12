namespace AdventOfCode.Utils.Bits;

public class UnsignedBitArray
{
    public ulong Data;

    public UnsignedBitArray(ulong data)
    {
        Data = data;
    }

    public bool this[int index]
    {
        get
        {
            ValidateIndex(index);
            return (Data & (1UL << index)) != 0;
        }
        set
        {
            ValidateIndex(index);
            if (value)
            {
                Data |= (1UL << index); // Set bit to 1
            }
            else
            {
                Data &= ~(1UL << index); // Set bit to 0
            }
        }
    }

    public override string ToString()
    {
        var bitRepresentation = new char[64];

        for (var i = 0; i < 64; i++)
        {
            bitRepresentation[63 - i] = this[i] ? '1' : '0';
        }

        return new string(bitRepresentation);
    }

    public string ToFixedString(int maxLength = 64)
    {
        var bitRepresentation = new char[maxLength];

        for (var i = 0; i < maxLength; i++)
        {
            bitRepresentation[maxLength - 1 - i] = this[i] ? '1' : '0';
        }

        return new string(bitRepresentation);
    }

    private static void ValidateIndex(int index)
    {
        if (index is < 0 or >= 64)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 63.");
        }
    }

    public UnsignedBitArray Clone() => new(Data);
}