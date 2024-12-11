using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class IntroScene : MonoBehaviour
{

    public Button StartButton;

    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne");
    }


}
