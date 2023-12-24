namespace StandSystem.Core;

public static class Decoder
{
    public static string HexToBinary(string hex)
    {
        try
        {
            return hex.ToCharArray().Aggregate("", (seed, c) => seed + HexToBinaryChar(c));
        }
        catch(ArgumentException e)
        {
            throw new ArgumentException($"{e.Message} - {hex}");
        }
    }

    public static string BinaryToHex(string binary)
    {
        while (binary.Length % 4 != 0)
        {
            binary += '0';
        }

        string result = "";

        for (int i = 0; i < binary.Length; i += 4)
        {
            result += BinaryToHexChar(binary.Substring(i, i + 4));
        }

        return result;
    }

    private static string HexToBinaryChar(char hex)
    {
        switch (hex)
        {
            case '0': return "0000";
            case '1': return "0001";
            case '2': return "0010";
            case '3': return "0011";
            case '4': return "0100";
            case '5': return "0101";
            case '6': return "0110";
            case '7': return "0111";
            case '8': return "1000";
            case '9': return "1001";
            case 'a': return "1010";
            case 'A': return "1010";
            case 'b': return "1011";
            case 'B': return "1011";
            case 'c': return "1100";
            case 'C': return "1100";
            case 'd': return "1101";
            case 'D': return "1101";
            case 'e': return "1110";
            case 'E': return "1110";
            case 'f': return "1111";
            case 'F': return "1111";
            default: return "";
        }
    }

    private static char BinaryToHexChar(string binary)
    {
        switch (binary)
        {
            case "0000": return '0';
            case "0001": return '1';
            case "0010": return '2';
            case "0011": return '3';
            case "0100": return '4';
            case "0101": return '5';
            case "0110": return '6';
            case "0111": return '7';
            case "1000": return '8';
            case "1001": return '9';
            case "1010": return 'A';
            case "1011": return 'B';
            case "1100": return 'C';
            case "1101": return 'D';
            case "1110": return 'E';
            case "1111": return 'F';
            default: return '0';
        }
    }
}
