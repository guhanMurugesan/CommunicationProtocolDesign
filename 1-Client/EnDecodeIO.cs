using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_Client
{
    class EnDecodeIO
    {
        public static decimal ReadDecimalAsBCD(BinaryReader input, int length, int precision)
        {
            decimal result = 0m;
            byte[] data = input.ReadBytes(length);
            for (int i = length - 1; i >= 0; i--) result = (result * 100m) + (data[i] >> 4) * 10 + (data[i] & 0x0F);
            return result * new decimal(1, 0, 0, false, (byte)precision);
        }

        public static void WriteDecimalAsBCD(BinaryWriter output, decimal value, int length, int precision)
        {
            if (value < 0) throw new ArgumentOutOfRangeException("value");

            byte[] data = new byte[length];
            decimal temp = Decimal.Truncate(value / new decimal(1, 0, 0, false, (byte)precision));
            for (int i = 0; i < length; i++)
            {
                int lastTwoDigits = (int)(temp % 100m);
                temp = Decimal.Floor(temp / 100);
                data[i] = (byte)(((lastTwoDigits / 10) << 4) | (lastTwoDigits % 10));
            }

            if (temp != 0m) throw new ArgumentOutOfRangeException("value");
            output.Write(data);
        }

    }
}
