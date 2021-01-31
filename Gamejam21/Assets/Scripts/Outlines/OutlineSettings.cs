using UnityEngine;

namespace Outlines
{
    [CreateAssetMenu(fileName = "OutlineSettings")]
    public class OutlineSettings : ScriptableObject
    {
        public float Width;
        public Outline.Mode Mode;
        public Color Color;

    }
}