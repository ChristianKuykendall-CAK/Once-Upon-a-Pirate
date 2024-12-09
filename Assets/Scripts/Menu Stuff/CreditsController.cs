using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public Button Next1;
    public Button Next2;
    public Button CoinCounter;

    public Image Dev;
    public Image Test;
    public Image Thanks;

    public void NextScreen1()
    {
        Dev.enabled = false;
        Test.enabled = true;
        Next1.enabled = false;
        Next1.image.enabled = false;
        Next2.enabled = true;
        Next2.image.enabled = true;
    }

    public void NextScreen2()
    {       
        Test.enabled = false;
        Thanks.enabled = true;
        Next2.enabled = false;
        Next2.image.enabled = false;
        CoinCounter.enabled = true;
        CoinCounter.image.enabled = true;
    }

    public void LoadMenu() //clicking the menu button will load the main menu
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadCoin() //clicking the menu button will load the main menu
    {
        SceneManager.LoadScene("CoinsCollected");
    }

}
