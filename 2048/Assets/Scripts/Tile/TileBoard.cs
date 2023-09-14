using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInput;

namespace Tile
{
    public class TileBoard : MonoBehaviour
    {
        [SerializeField]
        private Tile _tilePrefab;
        [SerializeField]
        private float _pauseMove = 0.1f;

        [SerializeField]
        private TileUIState[] _tileStates;

        private TileGrid grid;
        private List<Tile> tiles;

        private IInputController _inputController;
        private bool _isPause;

        public event Action<int> OnAddPoints;
        public event Action OnHaveTMove;

        private void Awake()
        {
            grid = GetComponentInChildren<TileGrid>();
            tiles = new List<Tile>(16);


            //ToDo при необходимости менять
            _inputController = new UserInputKeyboard();
        }

        public void ClearBoard()
        {
            foreach (var cell in grid.Cells)
            {
                cell.Tile = null;
            }

            foreach (var tile in tiles)
            {
                Destroy(tile.gameObject);
            }

            tiles.Clear();
        }

        public void CreateTile()
        {
            Tile tile = Instantiate(_tilePrefab, grid.transform);
            tile.SetState(_tileStates[0]);
            tile.Spawn(grid.GetRandomEmptyCell());
            tiles.Add(tile);
        }

        private void Update()
        {
            //ToDo не обязательно, но можно вынести в GameManager или еще одну прослойку
            if (!_isPause)
            {
                if (_inputController.InputUp()) Move(Vector2Int.up, 0, 1, 1, 1);

                if (_inputController.InputLeft()) Move(Vector2Int.left, 1, 1, 0, 1);
                
                if (_inputController.InputDown()) Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
                
                if (_inputController.InputRight()) Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
            }
        }

        private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
        {
            bool changed = false;

            for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
            {
                for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
                {
                    TileCell cell = grid.GetCell(x, y);

                    if (cell.Occupied)
                    {
                        changed |= MoveTile(cell.Tile, direction);
                    }
                }
            }

            if (changed)
            {
                StartCoroutine(WaitForChanges());
            }
        }

        private bool MoveTile(Tile tile, Vector2Int direction)
        {
            TileCell newCell = null;
            TileCell adjacent = grid.GetAdjacentCell(tile.Cell, direction);

            while (adjacent != null)
            {
                if (adjacent.Occupied)
                {
                    if (CanMerge(tile, adjacent.Tile))
                    {
                        MergeTiles(tile, adjacent.Tile);
                        return true;
                    }

                    break;
                }

                newCell = adjacent;
                adjacent = grid.GetAdjacentCell(adjacent, direction);
            }

            if (newCell != null)
            {
                tile.MoveTo(newCell);
                return true;
            }

            return false;
        }

        private bool CanMerge(Tile a, Tile b)
        {
            return a.State == b.State && !b.Locked;
        }

        private void MergeTiles(Tile a, Tile b)
        {
            tiles.Remove(a);
            a.Merge(b.Cell);

            int index = Mathf.Clamp(IndexOf(b.State) + 1, 0, _tileStates.Length - 1);
            TileUIState newState = _tileStates[index];

            b.SetState(newState);
            OnAddPoints?.Invoke(newState.Number);
        }

        private int IndexOf(TileUIState state)
        {
            for (int i = 0; i < _tileStates.Length; i++)
            {
                if (state == _tileStates[i])
                {
                    return i;
                }
            }

            return -1;
        }

        private IEnumerator WaitForChanges()
        {
            _isPause = true;

            yield return new WaitForSeconds(_pauseMove);

            _isPause = false;

            foreach (var tile in tiles)
            {
                tile.Locked = false;
            }

            if (tiles.Count != grid.Size)
            {
                CreateTile();
            }

            if (CheckForGameOver())
            {
                OnHaveTMove?.Invoke();
            }
        }

        private bool CheckForGameOver()
        {
            if (tiles.Count != grid.Size)
            {
                return false;
            }

            //ToDo как то упросить. плохо получилось. но ща хз как еще
            foreach (var tile in tiles)
            {
                TileCell up = grid.GetAdjacentCell(tile.Cell, Vector2Int.up);
                TileCell down = grid.GetAdjacentCell(tile.Cell, Vector2Int.down);
                TileCell left = grid.GetAdjacentCell(tile.Cell, Vector2Int.left);
                TileCell right = grid.GetAdjacentCell(tile.Cell, Vector2Int.right);

                if (up != null && CanMerge(tile, up.Tile))
                {
                    return false;
                }

                if (down != null && CanMerge(tile, down.Tile))
                {
                    return false;
                }

                if (left != null && CanMerge(tile, left.Tile))
                {
                    return false;
                }

                if (right != null && CanMerge(tile, right.Tile))
                {
                    return false;
                }
            }

            return true;
        }
    }
}