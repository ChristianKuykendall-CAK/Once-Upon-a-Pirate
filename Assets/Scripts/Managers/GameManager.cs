using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum Level { LevelOne, LevelTwo };
    public Level LevelNum;

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
    private void Start()
    {
        LevelNum = Level.LevelOne;
    }

    void FixedUpdate()
    {
        if (health > 100)
            health = 100;
        if (health < 0)
            health = 0;
    }

    public void Save()
    {
        playerTransform = GameObject.FindWithTag("Player").transform.position;

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

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/player.save");
        bf.Serialize(fileStream, theData);
        fileStream.Close();
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
            playerTransformBarrier = playerTransform + new Vector3(5f, 0, 0);

            pickedUpItems = new HashSet<string>(theData.pickedUpItems);
            currentEnemies = new HashSet<string>(theData.currentEnemies);

            DisableDefeatedEnemies();
        }
    }

    public bool HasItemBeenPickedUp(string itemID)
    {
        return pickedUpItems.Contains(itemID);
    }

    public void MarkItemAsPickedUp(string itemID)
    {
        if (!pickedUpItems.Contains(itemID))
        {
            pickedUpItems.Add(itemID);
        }
    }

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
