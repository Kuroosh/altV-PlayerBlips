using AltV.Net;
using AltV.Net.Elements.Entities;
using System;

namespace PlayerBlips
{
    public class PlayerBlipEntity : Player
    {
        public int Color { get; set; }
        public PlayerBlipEntity(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            Color = Main.StandardColor;
        }
    }
    public class MyPlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr playerPointer, ushort id)
        {
            return new PlayerBlipEntity(playerPointer, id);
        }
    }
}
