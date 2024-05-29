using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Preloading : MonoBehaviour
{
    // Start is called before the first frame update

    public Image filler;
    public Text Counter;

    static int firsttime = 0;
    float lerpAmount = 0f;

    // DeleteMe
    public bool IsGameTesting = false;
    void Start()
    {
        if (IsGameTesting)
        {
            PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash") + 100000);
        }
        firsttime = PlayerPrefs.GetInt("firsttime", firsttime);
        if (firsttime == 0)
        {

            PlayerPrefs.SetString("username", "Hunter");
            PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash") + 500);
            if (SystemInfo.systemMemorySize > 2000)
            {
                PlayerPrefs.SetInt("gfxGraphics", 1);
                firsttime = 1;
                PlayerPrefs.SetInt("firsttime", firsttime);
            }
            else
            {
                PlayerPrefs.SetInt("gfxGraphics", 0);
                firsttime = 1;
                PlayerPrefs.SetInt("firsttime", firsttime);
            }
        }
       
        StartCoroutine(startgame(Random.Range(3, 5)));
    }

   IEnumerator startgame(float seconds)
    {
        float animation = 0f;
        while (animation < seconds)
        {
            animation += Time.deltaTime;
            float lerpvalue = animation / seconds;
            lerpAmount = Mathf.Lerp(0, 1, lerpvalue);
            filler.fillAmount = lerpAmount;
            Counter.text = (int)(lerpAmount * 100) + "%";
            yield return null;

        }
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(1);

    }
}
