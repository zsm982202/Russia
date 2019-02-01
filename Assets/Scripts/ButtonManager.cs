using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public Text t1;
    public GameObject menu_scene;
    public GameObject rank_scene;

    public GameObject pauseButton;
    public Texture2D pauseImage;
    public Texture2D continueImage;

    public void OnPlayButtonDown() {
        SceneManager.LoadScene(1);
    }

    public void OnRankButtonDown() {
        menu_scene.SetActive(false);
        rank_scene.SetActive(true);
        RankManager._rankManager.ShowRanks();
    }

    public void OnRankReturnButtonDown() {
        menu_scene.SetActive(true);
        rank_scene.SetActive(false);
    }

    public void OnExitButtonDown() {
        Application.Quit();
    }

    public void OnReturnMenuButtonDown() {
        SceneManager.LoadScene(0);
    }

    public void OnPauseButtoonDown() {
        if (GameManager._gameManager.isPause == false) {
            GameManager._gameManager.isPause = true;
            Time.timeScale = 0;
            Sprite s = Sprite.Create(continueImage, new Rect(0, 0, continueImage.width, continueImage.height), Vector2.zero);
            pauseButton.GetComponent<Image>().sprite = s;
        } else if (GameManager._gameManager.isPause) {
            GameManager._gameManager.isPause = false;
            Time.timeScale = 1;
            Sprite s = Sprite.Create(pauseImage, new Rect(0, 0, pauseImage.width, pauseImage.height), Vector2.zero);
            pauseButton.GetComponent<Image>().sprite = s;
        }
    }
}
