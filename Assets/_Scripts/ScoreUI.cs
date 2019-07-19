using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private void Start()
    {
        Game.ScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }
}
