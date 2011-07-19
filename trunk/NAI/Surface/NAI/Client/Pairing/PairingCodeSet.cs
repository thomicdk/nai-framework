using System;
using System.Diagnostics;

namespace NAI.Client.Pairing
{
    internal struct PairingCodeSet
    {
        public readonly PairingCode PinCode;
        public readonly PairingCode ColorCode;

        private PairingCodeSet(string pinCode, string colorCode)
        {
            PinCode = new PairingCode(PairingCodeType.PIN_CODE, pinCode);
            ColorCode = new PairingCode(PairingCodeType.COLOR_CODE, colorCode);
        }

        public static PairingCodeSet GenerateRandom()
        {
            string pinCode = GenerateUniquePinCode();
            string colorCode = GenerateUniqueColorCode();
            return new PairingCodeSet(pinCode, colorCode);
        }

        private static string GenerateUniquePinCode()
        {
            Random random = new Random();
            string returnStr = "";
            do
            {
                int number = random.Next(0, (int)Math.Pow(10, PairingState.PIN_CODE_LENGTH) - 1);
                returnStr = number.ToString("D" + PairingState.PIN_CODE_LENGTH); // Prepend 0's
                //Debug.WriteLine(string.Format("PinCodeGenerator: '{0}'", returnStr));
            } while (ClientSessionsController.Instance.IsPairingCodeInUse(PairingCodeType.PIN_CODE, returnStr));
            Debug.WriteLineIf(DebugSettings.DEBUG_PAIRING, "Pin code generated: '" + returnStr + "'");
            return returnStr;
        }

        private static string GenerateUniqueColorCode()
        {
            // TODO: Implement when color codes are supported
            return string.Empty;
        }
    }
}
