using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    public Text FlavourText;
    public List<string> FlavourTexts;

    // Use this for initialization
    void Start()
    {
        int index = Random.Range(0, FlavourTexts.Count);
        FlavourText.text = FlavourTexts[index];
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
