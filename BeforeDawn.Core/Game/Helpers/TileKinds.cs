namespace BeforeDawn.Core.Game.Helpers
{
    class TileKinds
    {
        public static readonly string Default = "0";
        public static readonly string Start = "S";
        public static readonly string End = "E";
        public static readonly string Block = "B";
        public static readonly string Valuable = "$";
        public static readonly string ConveyorUp = "C1";
        public static readonly string ConveyorDown = "C2";
        public static readonly string ConveyorLeft = "C3";
        public static readonly string ConveyorRight = "C4";
        public static readonly string Teleport = "T";
        public static readonly string DoorRed = "D1";
        public static readonly string DoorBlue = "D2";
        public static readonly string DoorGreen = "D3";
        public static readonly string DoorYellow = "D4";
        public static readonly string KeyRed = "K1";
        public static readonly string KeyBlue = "K2";
        public static readonly string KeyGreen = "K3";
        public static readonly string KeyYellow = "K4";

        public static readonly string[] Doors = {DoorRed, DoorBlue, DoorGreen, DoorYellow};
        public static readonly string[] Keys = {KeyRed, KeyBlue, KeyGreen, KeyYellow};
        public static readonly string[] Collectables = {Valuable, DoorRed, DoorBlue, DoorGreen, DoorYellow, KeyRed, KeyBlue, KeyGreen, KeyYellow};
    }
}
