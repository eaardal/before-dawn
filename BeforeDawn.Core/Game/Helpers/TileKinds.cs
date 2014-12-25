using System.Collections.Generic;

namespace BeforeDawn.Core.Game.Helpers
{
    class TileKinds
    {
        public static readonly string Default = "--";
        public static readonly string Start = "SS";
        public static readonly string End = "EE";
        public static readonly string Block = "BB";
        public static readonly string Valuable = "$$";
        public static readonly string Teleport = "TT";

        public static readonly string ConveyorUp = "C1";
        public static readonly string ConveyorDown = "C2";
        public static readonly string ConveyorLeft = "C3";
        public static readonly string ConveyorRight = "C4";
        public static readonly string[] Conveyors = { ConveyorUp, ConveyorDown, ConveyorLeft, ConveyorRight };

        public static readonly string DoorRed = "D1";
        public static readonly string DoorBlue = "D2";
        public static readonly string DoorGreen = "D3";
        public static readonly string DoorYellow = "D4";
        public static readonly string[] Doors = { DoorRed, DoorBlue, DoorGreen, DoorYellow };

        public static readonly string KeyRed = "K1";
        public static readonly string KeyBlue = "K2";
        public static readonly string KeyGreen = "K3";
        public static readonly string KeyYellow = "K4";
        public static readonly string[] Keys = { KeyRed, KeyBlue, KeyGreen, KeyYellow };

        public static readonly string HazardFire = "H1";
        public static readonly string HazardWater = "H2";
        public static readonly string[] Hazards = { HazardWater, HazardFire };

        public static readonly string HazardProtectionFire = "P1";
        public static readonly string HazardProtectionWater = "P2";
        public static readonly string[] HazardProtections = {HazardProtectionFire, HazardProtectionWater};

        public static readonly IEnumerable<string> Collectables;

        static TileKinds()
        {
            var collectables = new List<string>();
            collectables.AddRange(Doors);
            collectables.AddRange(Keys);
            collectables.Add(Valuable);
            collectables.AddRange(HazardProtections);
            Collectables = collectables;
        }
    }
}
