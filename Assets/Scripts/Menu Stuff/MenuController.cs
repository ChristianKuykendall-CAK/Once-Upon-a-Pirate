using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Button startButton;
    public Button endButton;



    void Start()
    {
        //create on click listeners for start and end buttons
        startButton.onClick.AddListener(StartGame);
        endButton.onClick.AddListener(EndGame);
    }

    public void StartGame() //starts game when the start button is pressed
    {
        SceneManager.LoadScene("TestingEnvironment");
    }

    public void EndGame() //closes game when the quit button is pressed
    {
        Application.Quit();
    }
}