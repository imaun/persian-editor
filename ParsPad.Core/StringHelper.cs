using System;

namespace Farcin.Editor.Core {
    public static class StringHelper {

        //From .NET source code!
        public static bool IsNullOrWhiteSpace(string data) {
            if (data == null) return true;

            for (int i = 0; i < data.Length; i++) {
                if (!Char.IsWhiteSpace(data[i])) return false;
            }

            return true;
        }

        public static string GetHalfSpace() {
            Char NonBrakingSpace = (char)0x00a0;
            char x = (char)0x200c;

            return $"{x}";
        }
    }
}
