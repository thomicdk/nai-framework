
namespace NAI.Client.Pairing
{
    internal class PairingCode
    {
        public PairingCodeType Type { get; private set; }
        public string Code { get; private set; }

        public int Length
        {
            get
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    return Code.Length;
                }
                return 0;
            }
        }

        public PairingCode(PairingCodeType type, string code)
        {
            this.Type = type;
            this.Code = code;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Code))
            {
                return string.Format("{0}: ''", Type);
            }
            return string.Format("{0}: '{1}'", Type, Code);
        }

    }
}
