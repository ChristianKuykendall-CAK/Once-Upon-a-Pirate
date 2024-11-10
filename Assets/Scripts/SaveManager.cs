using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SaveManager
{
    public int ammo;
    public int health;
    public int coin;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public List<string> pickedUpItems;
    public List<string> currentEnemies;
}
