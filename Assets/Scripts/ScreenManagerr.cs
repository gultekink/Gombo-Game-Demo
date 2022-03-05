using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManagerr : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] public BotController[] _botController;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _highestScoreText;
    private int _highScore;
    private int _score;
    private string _name;

    void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _botController[0] = GameObject.FindGameObjectWithTag("Bot").GetComponent<BotController>();

    }

    void Update()
    {
        HighScore();

        if (_playerController.Score > _highScore)
        {
            _highScore = _playerController.Score;
            _name = _playerController.name;
        }

        if (_playerController._isGameFinish)
        {
            _highestScoreText.enabled = true;
            _highestScoreText.text = _name + " " + _highScore;
        }

        _scoreText.text = "Score: " + _playerController.Score;

    }

    void HighScore()
    {
        for (int i = 0; i < _botController.Length - 1; i++)
        {
            _highScore = _botController[i].Score;
            if (_botController[i + 1].Score > _botController[i].Score)
            {
                _highScore = _botController[i + 1].Score;
                _name = _botController[i + 1].name;
            }
        }

    }
}
