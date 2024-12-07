using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public Text coinText;
    public Slider coinSlider;
    private int totalCoins;

    private AudioSource Audio;
    public AudioClip coinSound;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            totalCoins = GameManager.instance.coin;
            coinSlider.maxValue = 100;
            coinSlider.value = 0;
        }
        Audio = GetComponent<AudioSource>();
    }

    public void ClicktoCount()
    {
        if (GameManager.instance != null)
        {
            coinText.text = "Coins: " + GameManager.instance.coin;
            StartCoroutine(CountCoins());
        }
    }

    private IEnumerator CountCoins()
    {
        int currentCount = 0;
        float elapsedTime = 0f;

        while (elapsedTime < 4f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 4f;

            // Use interpolation to calculate the current coin count
            currentCount = Mathf.RoundToInt(Mathf.Lerp(0, totalCoins, t));

            // Update UI
            coinText.text = "Coins: " + currentCount.ToString();
            coinSlider.value = Mathf.Lerp(0, totalCoins, t);

            Audio.PlayOneShot(coinSound);

            yield return null;
        }

        // Makes sure final coint displayed
        coinText.text = "Coins: " + totalCoins.ToString();
        coinSlider.value = GameManager.instance.coin;
    }
}
