using System.Collections;
using System.Collections.Generic;

namespace UnityRTSCore
{
    public enum ScreenEdgeDirection
    {
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        RightUp,
        RightDown,
        LeftDown,
        Neutral
    }

    public static class RTSCore
    {
        public static string MultiDebugLine(params object[] items)
        {
            string rv = "";
            int counter = 0;

            foreach (object item in items)
            {
                if(counter < 1)
                {
                    rv += item.ToString() + ": ";
                    counter++;
                }else
                {

                    rv += item.ToString() + "\n";
                    counter = 0;
                }
                
            }

            return rv;

        }



    }
}

