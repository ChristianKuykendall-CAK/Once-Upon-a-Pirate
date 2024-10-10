using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Button startButton;
    public Button loadButton;
    public Button controlsButton;
    public Button endButton;



    void Start()
    {
        //create on click listeners for start and end buttons
        startButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
        controlsButton.onClick.AddListener(ControlsScreen);
        endButton.onClick.AddListener(EndGame);
    }

    public void StartGame() //starts game when the start button is pressed
    {
        SceneManager.LoadScene("TestingEnvironment");
    }

    public void LoadGame() //loads the saved game when the load button is pressed
    {
       
    }
   
    public void ControlsScreen() //opens the controls view screen when the controls button is pressed
    {
        SceneManager.LoadScene("Controls");
    }

    public void EndGame() //closes game when the quit button is pressed
    {
        Application.Quit();
    }
}