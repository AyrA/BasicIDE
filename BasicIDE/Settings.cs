using System;
using System.Drawing;
using System.IO;
using Serial = System.IO.Ports;
using System.Linq;
using System.Xml.Serialization;

namespace BasicIDE
{
    [Serializable]
    public class Settings : IValidateable, ICloneable
    {
        public FontInfo EditorFont { get; set; }

        public SerialInfo SerialSettings { get; set; }

        public Settings()
        {
            EditorFont = new FontInfo("Courier New", 12f, FontStyle.Regular);
            SerialSettings = new SerialInfo();
        }

        public string Serialize()
        {
            var Ser = new XmlSerializer(typeof(Settings));
            using (var SW = new StringWriter())
            {
                Ser.Serialize(SW, this);
                return SW.ToString();
            }
        }

        public static Settings Deserialize(string Data)
        {
            var Ser = new XmlSerializer(typeof(Settings));
            using (var SR = new StringReader(Data))
            {
                return (Settings)Ser.Deserialize(SR);
            }
        }

        public void Validate()
        {
            if (EditorFont == null)
            {
                throw new ValidationException(nameof(EditorFont), "Value cannot be null");
            }
            if (SerialSettings == null)
            {
                throw new ValidationException(nameof(SerialSettings), "Value cannot be null");
            }
            EditorFont.Validate();
            SerialSettings.Validate();
        }

        public object Clone()
        {
            return new Settings()
            {
                EditorFont = (FontInfo)EditorFont.Clone(),
                SerialSettings = (SerialInfo)SerialSettings.Clone()
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Settings S)
            {
                if (S.EditorFont == null && EditorFont != null)
                {
                    return false;
                }
                if (S.SerialSettings == null && SerialSettings != null)
                {
                    return false;
                }
                return S.EditorFont.Equals(EditorFont) && S.SerialSettings.Equals(SerialSettings);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return
                EditorFont.GetHashCode() ^
                SerialSettings.GetHashCode() ^
                0x382FE4E4;
        }
    }

    [Serializable]
    public class SerialInfo : IValidateable, ICloneable
    {
        public const int AutoRate = 0;

        /// <summary>
        /// Baud rate
        /// </summary>
        public int BaudRate { get; set; }
        /// <summary>
        /// Parity
        /// </summary>
        public string Parity { get; set; }
        /// <summary>
        /// Stop bits
        /// </summary>
        public int StopBits { get; set; }
        /// <summary>
        /// Use Xon/Xoff protocol
        /// </summary>
        public bool XonXoff { get; set; }
        /// <summary>
        /// Cable with only TX and RX lines
        /// </summary>
        public bool PrimitiveCable { get; set; }

        public SerialInfo()
        {
            BaudRate = AutoRate;
            Parity = "Odd";
            StopBits = 1;
            XonXoff = false;
            PrimitiveCable = false;
        }

        public int GetAutoRate(bool IsBasic, bool IsSendToTrs)
        {
            if (BaudRate != AutoRate)
            {
                return BaudRate;
            }
            if (IsBasic)
            {
                if (IsSendToTrs)
                {
                    return 600;
                }
            }
            else
            {
                if (IsSendToTrs)
                {
                    return 300;
                }
            }
            return Tools.BaudRates.Max();
        }

        public string GetTrsSerialConfig(bool IsBasic, bool IsSendToTrs)
        {
            Validate();
            var Rate = GetAutoRate(IsBasic, IsSendToTrs);
            if (!Enum.TryParse(Parity, out Serial.Parity P))
            {
                P = Serial.Parity.Space;
            }
            return Tools.GetPortString(
                Rate, 8, P,
                StopBits == 1 ? Serial.StopBits.One : Serial.StopBits.Two,
                XonXoff ? Serial.Handshake.XOnXOff : Serial.Handshake.RequestToSend);
        }

        public Serial.SerialPort GetPort(string PortName, bool IsBasic, bool IsSendToTrs)
        {
            if (!Enum.TryParse(Parity, out Serial.Parity P))
            {
                P = Serial.Parity.Space;
            }
            Serial.Handshake HS;
            if (XonXoff)
            {
                if (PrimitiveCable)
                {
                    HS = Serial.Handshake.XOnXOff;
                }
                else
                {
                    HS = Serial.Handshake.RequestToSendXOnXOff;
                }
            }
            else
            {
                if (PrimitiveCable)
                {
                    HS = Serial.Handshake.None;
                }
                else
                {
                    HS = Serial.Handshake.RequestToSend;
                }
            }
            var SB = StopBits == 1 ? Serial.StopBits.One : Serial.StopBits.Two;
            var SP = new Serial.SerialPort(PortName, GetAutoRate(IsBasic, IsSendToTrs), P, 8, SB)
            {
                Handshake = HS,
                WriteBufferSize = 2,
                NewLine = "\r\n",
                WriteTimeout = 5000,
                Encoding = System.Text.Encoding.Default
            };
            return SP;
        }

        public void Validate()
        {
            if (BaudRate != 0 && !Tools.BaudRates.Contains(BaudRate))
            {
                throw new ValidationException(nameof(BaudRate), "Baud rate is invalid");
            }
            if (!Tools.Parity.Contains(Parity))
            {
                throw new ValidationException(nameof(Parity), "Parity is invalid");
            }
            if (!Tools.StopBits.Contains(StopBits))
            {
                throw new ValidationException(nameof(StopBits), "Stop bits is invalid");
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is SerialInfo S)
            {
                return S.BaudRate == BaudRate &&
                    S.Parity == Parity &&
                    S.PrimitiveCable == PrimitiveCable &&
                    S.StopBits == StopBits &&
                    S.XonXoff == XonXoff;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return BaudRate.GetHashCode() ^
                Parity.GetHashCode() ^
                PrimitiveCable.GetHashCode() ^
                StopBits.GetHashCode() ^
                XonXoff.GetHashCode() ^
                0x648F17C3;
        }
    }

    [Serializable]
    public class FontInfo : IValidateable, ICloneable
    {
        public string Name { get; set; }
        public float Size { get; set; }
        public FontStyle Style { get; set; }

        public FontInfo()
        {
            Name = "Courier New";
            Size = 10f;
            Style = FontStyle.Regular;
        }

        public FontInfo(string Name, float Size, FontStyle Style)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException($"'{nameof(Name)}' cannot be null or whitespace.", nameof(Name));
            }
            if (Size <= 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(Size));
            }

            this.Name = Name;
            this.Size = Size;
            this.Style = Style;
        }

        public FontInfo(Font font) : this(font.Name, font.Size, font.Style)
        {

        }

        public Font GetFont()
        {
            return new Font(Name, Size, Style);
        }

        public void Validate()
        {
            if (Size <= 0.0)
            {
                throw new ValidationException(nameof(Size), "Size too small");
            }

            if (string.IsNullOrEmpty(Name))
            {
                throw new ValidationException(nameof(Name), "Font name cannot be null or empty");
            }
            if (!FontFamily.Families.Any(m => m.Name == Name))
            {
                throw new ValidationException(nameof(Name), "Font with the given name does not exists");
            }
            if (!Tools.CheckValidFlags(Style))
            {
                throw new ValidationException(nameof(Style), "Invalid font style value");
            }

            try
            {
                GetFont().Dispose();
            }
            catch (Exception ex)
            {
                throw new ValidationException("Font", "Values do not yield a valid font. See inner exception for details.", ex);
            }
        }

        public object Clone()
        {
            return new FontInfo(Name, Size, Style);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is FontInfo I)
            {
                return Name == I.Name && Size == I.Size && Style == I.Style;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return
                Name.GetHashCode() ^
                Size.GetHashCode() ^
                Style.GetHashCode() ^
                0x1C44E37A;
        }
    }
}
