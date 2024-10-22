using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public enum ObjectPlaced { health, ammo, coin }
    public ObjectPlaced objectPlaced;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        // Check if this item has been picked up
        if (gameManager.HasItemBeenPickedUp(objectPlaced.ToString()))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.MarkItemAsPickedUp(objectPlaced.ToString());
            gameObject.SetActive(false); // Makes item disappear
        }
    }
}

