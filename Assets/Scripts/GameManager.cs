using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int health = 100;
    public int ammo = 5;
    public int coin = 0;

<<<<<<< HEAD
    public Text HealthText;
    public Text AmmoText;
    public Text CoinText;
=======
    
>>>>>>> origin/TestingEnvironment

    private HashSet<string> pickedUpItems = new HashSet<string>();

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
        if (health > 100)
            health = 100;
        //Prevents player health from going below 0
        if (health < 0)
            health = 0;

<<<<<<< HEAD
        HealthText.text = "Health: " + health;
        AmmoText.text = "Ammo: " + ammo;
        CoinText.text = "Coins: " + coin;
=======
        
>>>>>>> origin/TestingEnvironment
    }

    public void Save()
    {
        SaveManager theData = new SaveManager
        {
            health = health,
            ammo = ammo,
            coin = coin,
            pickedUpItems = new List<string>(pickedUpItems)
        };

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

            health = theData.health;
            ammo = theData.ammo;
            coin = theData.coin;

<<<<<<< HEAD
            pickedUpItems = new HashSet<string>(theData.pickedUpItems);
            Debug.Log("Save Path: " + Application.persistentDataPath);
            HealthText.text = "Health: " + health;
            AmmoText.text = "Ammo: " + ammo;
            CoinText.text = "Coins: " + coin;

            Debug.Log("Game loaded successfully.");
        }
    }

    public bool HasItemBeenPickedUp(string itemName)
    {
        return pickedUpItems.Contains(itemName);
    }

    public void MarkItemAsPickedUp(string itemName)
    {
        if (!pickedUpItems.Contains(itemName))
        {
            pickedUpItems.Add(itemName);
=======
            
>>>>>>> origin/TestingEnvironment
        }
    }
}
