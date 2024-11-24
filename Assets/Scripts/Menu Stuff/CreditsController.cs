using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void LoadMenu() //clicking the menu button will load the main menu
    {
        SceneManager.LoadScene("Menu");
    }
}
