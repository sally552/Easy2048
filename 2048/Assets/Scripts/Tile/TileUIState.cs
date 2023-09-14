using UnityEngine;

namespace Tile
{
    [CreateAssetMenu(menuName = "Tile State")]
    public class TileUIState : ScriptableObject
    {
        public Color BackgroundColor;
        public Color TextColor;
        public int Number;
    }
}
