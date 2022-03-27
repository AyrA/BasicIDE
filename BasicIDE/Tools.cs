using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Xml.Serialization;

namespace BasicIDE
{
    public static class Tools
    {
        private static readonly int[] baudRates = new int[]
        {
            75,
            110,
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            19200
        };

        private static readonly int[] stopBits = new int[] { 1, 2 };

        private const string parity = "None,Even,Odd,Ignore";

        public static int[] BaudRates { get => (int[])baudRates.Clone(); }
        public static int[] StopBits { get => (int[])stopBits.Clone(); }
        public static string[] Parity { get => parity.Split(','); }

        public static string ToXML<T>(this T type)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var SW = new StringWriter())
            {
                ser.Serialize(SW, type);
                return SW.ToString();
            }
        }

        public static T FromXML<T>(this string s)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var SR = new StringReader(s))
            {
                return (T)ser.Deserialize(SR);
            }
        }

        public static int IndexOf<T>(this T[] Array, T Value)
        {
            for (var i = 0; i < Array.Length; i++)
            {
                if (Array[i].Equals(Value))
                {
                    return i;
                }
            }
            return -1;
        }

        public static string GetPortString(int BaudRate, int DataBits, Parity P, StopBits S, Handshake H)
        {
            string Ret = "";

            var i = baudRates.IndexOf(BaudRate);
            if (i >= 0)
            {
                Ret += (i + 1).ToString();
            }
            else
            {
                throw new ArgumentException("Unsupported baud rate");
            }
            if (DataBits < 6 || DataBits > 8)
            {
                throw new ArgumentException("Unsupported number of data bits");
            }
            Ret += DataBits.ToString();
            switch (P)
            {
                case System.IO.Ports.Parity.None:
                case System.IO.Ports.Parity.Even:
                case System.IO.Ports.Parity.Odd:
                    Ret += P.ToString().Substring(0, 1);
                    break;
                case System.IO.Ports.Parity.Space:
                case System.IO.Ports.Parity.Mark:
                    Ret += "I";
                    break;
                default:
                    throw new ArgumentException("Invalid parity setting");
            }
            switch (S)
            {
                case System.IO.Ports.StopBits.One:
                    Ret += "1";
                    break;
                case System.IO.Ports.StopBits.Two:
                    Ret += "2";
                    break;
                case System.IO.Ports.StopBits.None:
                case System.IO.Ports.StopBits.OnePointFive:
                    throw new ArgumentException("none or 1.5 stop bits are not supported. Must be 1 or 2");
                default:
                    throw new ArgumentException("Invalid stop bit value");
            }
            switch (H)
            {
                case Handshake.None:
                case Handshake.RequestToSend:
                    Ret += "D";
                    break;
                case Handshake.XOnXOff:
                case Handshake.RequestToSendXOnXOff:
                    Ret += "E";
                    break;
                default:
                    throw new ArgumentException("Unknown handshake value");
            }
            return Ret;
        }

        public static string FormatSize(double Length)
        {
            var Sizes = ",K,M,G,T,P,E,Z,Y".Split(',');
            int index = 0;
            while (Length >= 1000.0 && index < Sizes.Length - 1)
            {
                Length /= 1000.0;
                ++index;
            }
            return Math.Round(Length, 2) + Sizes[index] + "B";
        }

        public static bool CheckValidFlags<T>(T Value) where T : Enum
        {
            //Filter out single entries immediately
            if (Enum.IsDefined(typeof(T), Value))
            {
                return true;
            }
            ulong i = 0;
            ulong v = EnumToLong(Value);
            foreach (var V in Enum.GetValues(typeof(T)).OfType<T>())
            {
                i |= EnumToLong(V);
            }
            return (v & i) == v;
        }

        public static ulong EnumToLong<T>(T Value) where T : Enum
        {
            var ValT = Enum.GetUnderlyingType(typeof(T));
            if (ValT == typeof(short) || ValT == typeof(sbyte) || ValT == typeof(int) || ValT == typeof(long))
            {
                return (ulong)Convert.ToInt64(Value);
            }
            return Convert.ToUInt64(Value);
        }
    }
}
