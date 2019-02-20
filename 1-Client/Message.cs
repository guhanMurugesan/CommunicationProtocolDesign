using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_Client
{
    interface IPublish
    {
        void TryPublish(BinaryWriter output);
    }

    public class CGMessage : IPublish
    {
        public UInt32 TransactionId;
        public string address;
        public MessageType messageType;
        public byte dataLength;
        public CompressionType compressionType;
        public EncryptionType encryptionType;
        public byte crc;


        public CGMessage()
        {
            TransactionId = Program.TransactionId;
            address = Program.IPAddress;
            messageType = Program.CurrentMessageType;
        }

        public CGMessage(BinaryReader input)
        {
            ParseHeader(input);
        }

        public CGMessage(CGMessage cgMessage)
        {
            this.TransactionId = cgMessage.TransactionId;
            this.address = cgMessage.address;
            this.messageType = cgMessage.messageType;
            this.compressionType = cgMessage.compressionType;
            this.encryptionType = cgMessage.encryptionType;
            this.dataLength = cgMessage.dataLength;
        }

        public static void TryParse(byte[] input, out CGMessage cgMessage)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(input));
            cgMessage = new CGMessage(reader);

            switch (cgMessage.messageType)
            {
                case MessageType.Type1:
                    cgMessage = Module1.ContinueParse(reader, cgMessage);
                    break;
                case MessageType.Type2:
                    cgMessage = Module2.ContinueParse(reader, cgMessage);
                    break;
                case MessageType.Type3:
                    cgMessage = Module3.ContinueParse(reader, cgMessage);
                    break;
                default:
                    break;
            }

            cgMessage.ParseTailingFields(reader);
        }


        private void ParseHeader(BinaryReader input)
        {
            this.TransactionId = (UInt32)EnDecodeIO.ReadDecimalAsBCD(input, 2, 0);
            this.address = Encoding.UTF8.GetString(input.ReadBytes(4));
            this.messageType = (MessageType)input.ReadByte();
            this.compressionType = (CompressionType)input.ReadByte();
            this.encryptionType = (EncryptionType)input.ReadByte();
            this.dataLength = input.ReadByte();
        }

        private void ParseTailingFields(BinaryReader input)
        {
            this.crc = input.ReadByte();
        }

        public virtual void TryPublish(BinaryWriter output)
        {
            this.PublishLeadingFields(output);
            this.PublishTrailingFields(output);
        }

        public void PublishLeadingFields(BinaryWriter output)
        {
            EnDecodeIO.WriteDecimalAsBCD(output, (decimal)this.TransactionId, 2, 0);
            output.Write(Encoding.UTF8.GetBytes(this.address.ToArray(), 0, 4));
            output.Write((byte)this.messageType);
            output.Write((byte)this.compressionType);
            output.Write((byte)this.encryptionType);
            output.Write(this.dataLength);
        }

        public void PublishTrailingFields(BinaryWriter output)
        {
            output.Write((byte)this.crc);
        }
    }

    public enum MessageType
    {
        Type1 = 0x00,
        Type2 = 0x01,
        Type3 = 0x02
    }

    public enum CompressionType
    {
        NoComperssion = 0x00
    }

    public enum EncryptionType
    {
        NoEncryption = 0x00,
        CG = 0xAA
    }

    interface ICGMessage
    {
        string value { get; set; }
    }

    public class Module1 : CGMessage,ICGMessage
    {
        public string value {get;set;}

        public Module1(CGMessage cgMessage)
            : base(cgMessage)
        {

        }

        public Module1(string value)
        {
            this.value = value;
            this.dataLength = (byte)value.Length;
        }

        public static Module1 ContinueParse(BinaryReader input, CGMessage cgMessage)
        {
            Module1 feature = new Module1(cgMessage);

            feature.value = Encoding.UTF8.GetString(input.ReadBytes(cgMessage.dataLength));

            return feature;
        }

        public override void TryPublish(BinaryWriter output)
        {
            this.PublishLeadingFields(output);
            this.PublishTrailingFields(output);
        }

        public void PublishLeadingFields(BinaryWriter output)
        {
            base.PublishLeadingFields(output);
            output.Write(Encoding.UTF8.GetBytes(this.value.ToArray(), 0, this.value.Length > 128 ? 128 : this.value.Length));
        }

        public void PublishTrailingFields(BinaryWriter output)
        {
            base.PublishTrailingFields(output);
        }
    }

    public class Module2 : CGMessage, ICGMessage
    {
        public string value { get; set; }

        public Module2(CGMessage cgMessage)
            : base(cgMessage)
        {

        }

        public Module2(string value)
        {
            this.value = value;
            this.dataLength = (byte)value.Length;
        }

        public static Module2 ContinueParse(BinaryReader input, CGMessage cgMessage)
        {
            Module2 feature = new Module2(cgMessage);

            feature.value = Encoding.UTF8.GetString(input.ReadBytes(cgMessage.dataLength));

            return feature;
        }

        public override void TryPublish(BinaryWriter output)
        {
            this.PublishLeadingFields(output);
            this.PublishTrailingFields(output);
        }

        public void PublishLeadingFields(BinaryWriter output)
        {
            base.PublishLeadingFields(output);
            output.Write(Encoding.UTF8.GetBytes(this.value.ToArray(), 0, this.value.Length > 256 ? 256 : this.value.Length));
        }

        public void PublishTrailingFields(BinaryWriter output)
        {
            base.PublishTrailingFields(output);
        }
    }

    public class Module3 : CGMessage, ICGMessage
    {
        public string value { get; set; }

        public Module3(CGMessage cgMessage)
            : base(cgMessage)
        {

        }

        public Module3(string value)
        {
            this.value = value;
            this.dataLength = (byte)value.Length;
        }

        public static Module3 ContinueParse(BinaryReader input, CGMessage cgMessage)
        {
            Module3 feature = new Module3(cgMessage);

            feature.value = Encoding.UTF8.GetString(input.ReadBytes(cgMessage.dataLength));

            return feature;
        }

        public override void TryPublish(BinaryWriter output)
        {
            this.PublishLeadingFields(output);
            this.PublishTrailingFields(output);
        }

        public void PublishLeadingFields(BinaryWriter output)
        {
            base.PublishLeadingFields(output);
            output.Write(Encoding.UTF8.GetBytes(this.value.ToArray(), 0, this.value.Length > 512 ? 512 : this.value.Length));
        }

        public void PublishTrailingFields(BinaryWriter output)
        {
            base.PublishTrailingFields(output);
        }
    }
}
