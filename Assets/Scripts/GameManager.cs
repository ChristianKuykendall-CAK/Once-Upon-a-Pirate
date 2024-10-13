using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        if(health > 100)
            health = 100;

        //Updates the player's health, ammo, and coin count every frame
        HealthText.text = "Health: " + health;
        AmmoText.text = "Ammo: " + ammo;
        CoinText.text = "Coins: " + coins;
    }
    public void Save()
    {
        SaveManager theData = new SaveManager()
        {
            health = health,
            ammo = ammo
        };
        //theData.currentRoom = NavigationManager.instance.currentRoom.name;
        theData.health = health; // health data
        theData.ammo = ammo; // health data

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/player.save");
        bf.Serialize(fileStream, theData);
        fileStream.Close();

        Debug.Log("Game saved successfully.");
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            SaveManager theData = (SaveManager)bf.Deserialize(fileStream);
            fileStream.Close();

            //NavigationManager.instance.SetCurrentRoom(theData.currentRoom);
            health = theData.health;
            ammo = theData.ammo;

            HealthText.text = "Health: " + health;
            AmmoText.text = "Ammo: " + ammo;
        }
    }
}
