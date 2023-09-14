using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        [SerializeField]
        private TextMeshProUGUI _hiscoreText;

        private const string SCORE_KEY = "SAVE_SCORE";
        private int _score;

        public void IncreaseScore(int points)
        {
            SetScore(_score + points);
        }

        public void ResetScore()
        {
            SetScore(0);
        }

        private void SetScore(int score)
        {
            this._score = score;
            _scoreText.text = score.ToString();

            SaveScore();
        }

        private void SaveScore()
        {
            int hiscore = LoadScore();

            if (_score > hiscore)
            {
                PlayerPrefs.SetInt(SCORE_KEY, _score);
            }
        }

        private int LoadScore()
        {
            return PlayerPrefs.GetInt(SCORE_KEY, 0);
        }
    }
}