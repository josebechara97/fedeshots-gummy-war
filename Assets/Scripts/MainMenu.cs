using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int bestScore;
    public int lastScore;
    public Text lastScoreTxt;
    public Text bestScoreTxt;

    // Start is called before the first frame update
    void Start()
    {
        lastScore = PlayerPrefs.GetInt("lastScore", 0);
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
        lastScoreTxt.text = "Last Score: " + lastScore;
        bestScoreTxt.text = "Best Score: " + bestScore;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
