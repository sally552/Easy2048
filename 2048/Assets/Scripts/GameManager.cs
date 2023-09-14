using System;
using System.Collections;
using Tile;
using UI;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TileBoard _board;

    [SerializeField]
    private Score _score;

    [SerializeField]
    private int _startTileCount = 2;
    [SerializeField]
    private GameStateUI _gameStateUI;



    private void Start()
    {
        _board.OnAddPoints += _score.IncreaseScore;
        _board.OnHaveTMove += GameOver;

        NewGame();
    }

    public void NewGame()
    {
        if (_board == null || gameObject == null  || _gameStateUI == null)
        {
            throw new Exception("Not found game components");
        }


        // reset score
        _score.ResetScore();

        // hide game over screen
        _gameStateUI.StartGame();

        // update board state
        _board.ClearBoard();
        for (int i = 0; i < _startTileCount; i++)
        {
            _board.CreateTile();
        }

        _board.enabled = true;
    }

    public void GameOver()
    {
        _board.enabled = false;
        _gameStateUI.GameOver();
    }

   


    private void OnDestroy()
    {
        _board.OnAddPoints -= _score.IncreaseScore;
        _board.OnHaveTMove -= GameOver;
    }

}
