using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverInterface : MonoBehaviour {
    public static GameOverInterface _gameOverInterface;
    private void Awake() {
        _gameOverInterface = this;
    }

    public bool isInterfaceChangeBlack = false;
    public bool isInterfaceChangeWhite = false;

    private void Update() {
        if(isInterfaceChangeBlack)
            DeltaTurnBlack();
        if (isInterfaceChangeWhite)
            DeltaTurnWhite();
    }

    public GameObject black_Image;
    public GameObject score_text;
    public GameObject return_Button;
    public GameObject return_Button2;
    public GameObject pause_Button;

    private void DeltaTurnBlack() {
        Color tempColor = black_Image.GetComponent<Image>().color;
        tempColor.a = Mathf.Lerp(tempColor.a, 1f, Time.deltaTime * 1f);
        black_Image.GetComponent<Image>().color = tempColor;

        Color tempColor2 = score_text.GetComponent<Text>().color;
        tempColor2.a = Mathf.Lerp(tempColor2.a, 1f, Time.deltaTime * 1f);
        score_text.GetComponent<Text>().color = tempColor2;

        Color tempColor3 = return_Button.GetComponentInChildren<Text>().color;
        tempColor3.a = Mathf.Lerp(tempColor3.a, 1f, Time.deltaTime * 1f);
        return_Button.GetComponentInChildren<Text>().color = tempColor3;

        if (Mathf.Abs(tempColor2.a - 1f) < 0.01f) {
            isInterfaceChangeBlack = false;
        }
    }

    private void DeltaTurnWhite() {
        Color tempColor = black_Image.GetComponent<Image>().color;
        tempColor.a = Mathf.Lerp(tempColor.a, 0, Time.deltaTime * 1f);
        black_Image.GetComponent<Image>().color = tempColor;

        Color tempColor2 = score_text.GetComponent<Text>().color;
        tempColor2.a = Mathf.Lerp(tempColor2.a, 0, Time.deltaTime * 1f);
        score_text.GetComponent<Image>().color = tempColor2;

        Color tempColor3 = return_Button.GetComponentInChildren<Text>().color;
        tempColor3.a = Mathf.Lerp(tempColor3.a, 0, Time.deltaTime * 1f);
        return_Button.GetComponentInChildren<Text>().color = tempColor3;

        if (Mathf.Abs(tempColor.a - 0) < 0.01f) {
            isInterfaceChangeWhite = false;
        }
    }

    public void TurnBlack() {
        return_Button.SetActive(true);
        pause_Button.SetActive(false);
        return_Button2.SetActive(false);
        score_text.GetComponent<Text>().text = "GameOver!\n\nYour Score is  " + ShowScoreInterface._showScoreInterface.score;
        isInterfaceChangeBlack = true;
    }

    public void TurnWhite() {
        isInterfaceChangeWhite = true;
    }

    public void InitNewInterface() {
        pause_Button.SetActive(true);
        return_Button2.SetActive(true);
    }
}
