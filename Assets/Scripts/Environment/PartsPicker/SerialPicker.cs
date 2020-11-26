using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class SerialPicker : WorldPartsPicker
    {
        public SerialPicker(List<Map2D> maps) : base(maps)
        {
            Position = -1;
        }

        public override Map2D Next()
        {
            Position++;
            return Current;
        }

        public bool CanTakeNext()
        {
            return Count > Position + 1;
        }
    }
}