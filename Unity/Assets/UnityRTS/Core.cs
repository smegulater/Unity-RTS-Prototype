using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum UnitType
    {
        NPC,
        TechInfantry,
        LightInfantry,
        MediumInfantry,
        HeavyInfantry,
        LightVehicle,
        MediumVehicle,
        HeavyVehicle,
        LightTech,
        MediumTech,
        HeavyTech,
        LightSupport,
        MediumSupport,
        HeavySupport
    }

    public static class RTSCore
    {
        private static Texture2D _whiteTexture;
        public static Texture2D WhiteTexture
        {
            get
            {
                if(_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();

                }
                return _whiteTexture;
            }
        }

        //public static void ChangeSelectionColor (Color color)
        //{
        //    _whiteTexture.SetPixel(0, 0, color);
        //    _whiteTexture.Apply();
        //}
        //
        // - NOT NEEDED recommend removal as the pixel color should stay white as color is added when drawing

        /// <summary>
        ///     used in the Debug Colsole to correctly format metric:value pairs to a string
        /// </summary>
        /// <param name="items">
        /// </param>
        /// <returns>
        ///     returns a foramtted metric:value pair for use in the DebugConsole
        /// </returns>
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

        public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            // Calculate corners
            var topLeft = Vector3.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;

        }
        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            DrawScreenRect(new Rect(rect.xMax -thickness,rect.yMin,thickness,rect.height), color);
            DrawScreenRect(new Rect(rect.xMin, rect.yMax -thickness, rect.width, thickness), color);
        }
        public static void DrawScreenRectBorderShaded(Rect rect, float thickness, Color color)
        {
            // Draw shaded box
            color.a = 0.35f;
            DrawScreenRect(rect, color);
            // Draw Border
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
        {
            var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
            var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;

            var bounds = new Bounds();
            bounds.SetMinMax(min, max);
            return bounds;
        }
    }
}

