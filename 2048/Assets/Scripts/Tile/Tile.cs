using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tile
{
    public class Tile : MonoBehaviour
    {
        public TileUIState State { get; private set; }
        public TileCell Cell { get; private set; }
        public bool Locked { get; set; }

        //плохо, но это часть префаба
        private Image _background;
        private TextMeshProUGUI _text;


        private void Awake()
        {
            _background = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetState(TileUIState state)
        {
            State = state;

            _background.color = state.BackgroundColor;
            _text.color = state.TextColor;
            _text.text = state.Number.ToString();
        }

        public void Spawn(TileCell cell)
        {
            ResetCell(cell);
            Cell.Tile = this;

            transform.position = cell.transform.position;
        }

        public void MoveTo(TileCell cell)
        {
            ResetCell(cell);
            Cell.Tile = this;

            StartCoroutine(Animate(cell.transform.position, false));
        }

        public void Merge(TileCell cell)
        {
            ResetCell();
            cell.Tile.Locked = true;

            StartCoroutine(Animate(cell.transform.position, true));
        }

        private void ResetCell(TileCell cell = null)
        {
            if (Cell != null)
            {
                Cell.Tile = null;
            }

            Cell = cell;
        }

        private IEnumerator Animate(Vector3 to, bool merging)
        {
            float elapsed = 0f;
            float duration = 0.1f;

            Vector3 from = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = to;

            if (merging)
            {
                Destroy(gameObject);
            }
        }

    }
}