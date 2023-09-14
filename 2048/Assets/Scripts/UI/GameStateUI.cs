using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class GameStateUI : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _gameOver;
        [SerializeField]
        private float _duration = 0.5f;

        public void StartGame()
        {
            _gameOver.alpha = 0f;
            _gameOver.interactable = false;
        }

        public void GameOver()
        {
            _gameOver.interactable = true;

            StartCoroutine(Fade(_gameOver, 1f, 1f));
        }


        private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            float elapsed = 0f;
            float from = canvasGroup.alpha;

            while (elapsed < _duration)
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / _duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = to;
        }
    }
}