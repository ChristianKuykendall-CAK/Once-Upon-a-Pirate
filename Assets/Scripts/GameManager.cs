using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //UI Variables
    public int health = 100;
    public int ammo = 5;
    public int coins = 0;

    //Text variables
    public Text HealthText;
    public Text AmmoText;
    public Text CoinText;

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    void FixedUpdate()
    {
        //Prevents player health from going above 100
        if (health > 100)
            health = 100;

        //Updates the player's health, ammo, and coin count every frame
        HealthText.text = "Health: " + health;
        AmmoText.text = "Ammo: " + ammo;
        CoinText.text = "Coins: " + coins;
    }
}
