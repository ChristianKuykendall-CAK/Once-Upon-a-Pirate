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
    public Vector3 playerTransform;
    public Vector3 playerTransformBarrier;

    private HashSet<string> pickedUpItems = new HashSet<string>();
    private HashSet<string> currentEnemies = new HashSet<string>();

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
        
    }

    public void Save()
    {
        playerTransform = GameObject.FindWithTag("Player").transform.position; // Used for putting back to checkpoint. VERY IMPORTANT

        // pulled all data that needs to be saved from the SaveManager script. 
        SaveManager theData = new SaveManager
        {
            health = health,
            ammo = ammo,
            coin = coin,
            playerPosX = playerTransform.x,
            playerPosY = playerTransform.y,
            playerPosZ = playerTransform.z,
            pickedUpItems = new List<string>(pickedUpItems),
            currentEnemies = new List<string>(currentEnemies)
        };

        // Saving stuff to a file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/player.save");
        bf.Serialize(fileStream, theData);
        fileStream.Close();

        //Debug.Log("Game saved successfully.");
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

            playerTransform = new Vector3(theData.playerPosX, theData.playerPosY, theData.playerPosZ);

            playerTransformBarrier = playerTransform + new Vector3(5f,0,0); // Used to put the player 5 units to the right of the checkpoint when loading in

            // Below are the HashSets that get the data of the items and enemies
            pickedUpItems = new HashSet<string>(theData.pickedUpItems);
            currentEnemies = new HashSet<string>(theData.currentEnemies);

            DisableDefeatedEnemies(); // used to get each enemy


            //Debug.Log("Save Path: " + Application.persistentDataPath);
            //Debug.Log("Game loaded successfully.");
        }
    }


    // Below code is used to save items and enemies to the hashsets individually

    public bool HasItemBeenPickedUp(string itemName)
    {
        return pickedUpItems.Contains(itemName);
    }

    public void MarkItemAsPickedUp(string itemName)
    {
        if (!pickedUpItems.Contains(itemName))
        {
            pickedUpItems.Add(itemName);
        }
    }

    // This finds each enemy with their unique gameobject name
    private void DisableDefeatedEnemies()
    {
        foreach (string enemyName in currentEnemies)
        {
            GameObject enemy = GameObject.Find(enemyName);
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
    }
    public void MarkEnemyAsDefeated(string enemyName)
    {
        if (!currentEnemies.Contains(enemyName))
        {
            currentEnemies.Add(enemyName);
        }
    }
}
