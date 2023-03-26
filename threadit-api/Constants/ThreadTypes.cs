using System;

namespace ThreaditAPI.Constants {
    public static class ThreadTypes {
        public const string TEXT = "text";
        public const string LINK = "link";
        public const string IMAGE = "image";

        public static readonly string[] types = new string[] {
            TEXT, LINK, IMAGE
        };

        public static readonly string typesString = String.Join(", ", types);
    }
}
