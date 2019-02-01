using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

    public static RankManager _rankManager;

    private void Awake() {
        _rankManager = this;
    }

    private string[] DateStr = new string[10];
    private int[] scores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public Text RanksText;

    public void UpdateRank(int x) {
        for (int i = 0; i < 10; i++) {
            scores[i] = PlayerPrefs.GetInt("score" + i, 0);
            DateStr[i] = PlayerPrefs.GetString("DateStr" + i, "");
        }
        if (x > scores[9]) {
            for (int i = 9; i > 0; i--) {
                if (x > scores[i - 1]) {
                    scores[i] = scores[i - 1];
                    DateStr[i] = DateStr[i - 1];
                } else {
                    scores[i] = x;
                    DateStr[i] = System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Day;
                    break;
                }
            }
            if (x > scores[0]) {
                scores[0] = x;
                DateStr[0] = System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Day;
            }
        }
        for (int i = 0; i < 10; i++) {
            PlayerPrefs.SetInt("score" + i, scores[i]);
            PlayerPrefs.SetString("DateStr" + i, DateStr[i]);
        }
    }

    public void ShowRanks() {
        for (int i = 0; i < 10; i++) {
            scores[i] = PlayerPrefs.GetInt("score" + i, 0);
            DateStr[i] = PlayerPrefs.GetString("DateStr" + i, "");
        }
        RanksText.text = "";
        for (int i = 0; i < 10; i++) {
            if (scores[i] > 0) {
                RanksText.text += "No." + (i + 1) + "       " + scores[i] + "       " + DateStr[i] + "\n";
            }
        }
        for (int i = 0; i < 10; i++) {
            PlayerPrefs.SetInt("score" + i, scores[i]);
            PlayerPrefs.SetString("DateStr" + i, DateStr[i]);
        }
    }
}
