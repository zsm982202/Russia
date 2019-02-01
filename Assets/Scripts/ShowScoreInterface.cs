using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScoreInterface : MonoBehaviour {
    public static ShowScoreInterface _showScoreInterface;
    private void Awake() {
        _showScoreInterface = this;
    }

    public Text ScoreText;
    public int score = 0;

    public void AddScore(int add_score) {
        score += add_score;
    }

    public void ShowScore() {
        ScoreText.text = "score: " + score;
    }
}
