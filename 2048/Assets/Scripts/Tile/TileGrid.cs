using UnityEngine;

namespace Tile
{
    public class TileGrid : MonoBehaviour
    {
        public TileRow[] Rows { get; private set; }
        public TileCell[] Cells { get; private set; }

        public int Size => Cells.Length;
        public int Height => Rows.Length;
        public int Width => Size / Height;

        private void Awake()
        {
            Rows = GetComponentsInChildren<TileRow>();
            Cells = GetComponentsInChildren<TileCell>();

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].Coordinates = new Vector2Int(i % Width, i / Width);
            }
        }

        public TileCell GetCell(int x, int y)
        {
            return (x >= 0 && x < Width && y >= 0 && y < Height) ?
                Rows[y].Cells[x] :
                null;
        }

        private TileCell GetCell(Vector2Int coordinates)
        {
            return GetCell(coordinates.x, coordinates.y);
        }



        public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
        {
            Vector2Int coordinates = cell.Coordinates;
            coordinates.x += direction.x;
            coordinates.y -= direction.y;

            return GetCell(coordinates);
        }

        public TileCell GetRandomEmptyCell()
        {
            int index = Random.Range(0, Cells.Length);
            int startingIndex = index;

            while (Cells[index].Occupied)
            {
                index++;

                if (index >= Cells.Length)
                {
                    index = 0;
                }


                if (index == startingIndex)
                {
                    return null;
                }
            }
            return Cells[index];
        }
    }
}