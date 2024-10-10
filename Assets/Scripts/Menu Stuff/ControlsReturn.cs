using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsReturn : MonoBehaviour
{

    public Button returnButton;
    
   
    void Start()
    {
        returnButton.onClick.AddListener(returnGame);
    }

    public void returnGame() //returns to the menu after the return button is clicked
    {
        SceneManager.LoadScene("Menu");
    }
    
}
