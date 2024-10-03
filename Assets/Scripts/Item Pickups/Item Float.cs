using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour
{

    
    public float distance = 1;
    public int movement = 2;
    public int amount = 5;

    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine("MoveObject");
    }


    //coroutine

    IEnumerator MoveObject()
    {
        while (true)
        {
            transform.Translate(new Vector2(0, distance * movement));
            amount--;

            if (amount <= 0)
            {
                movement *= -1;
                amount = 5;
            }

            yield return new WaitForSeconds(.2f);
        }
    }
}