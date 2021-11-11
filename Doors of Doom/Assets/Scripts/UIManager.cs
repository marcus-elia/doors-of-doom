using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI numSnowballsText;
    public TextMeshProUGUI levelText;

    public static int numSnowballs;
    public static int level;

    public GameObject endScreen;
    public TextMeshProUGUI resultMessage;

    private Dictionary<int, string> levelToResult;

    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
        numSnowballsText.text = "0";
        levelText.text = "0";

        levelToResult = new Dictionary<int, string>()
        {
            {0, "You got through 0 doors.\n That's adorable." },
            {1, "You got through 1 door.\n What a dork." },
            {2, "You got through 2 doors.\n You're a doorbell." },
            {3, "You got through 3 doors.\n All your life has been a series of doors in your face." },
            {4, "You got through 4 doors.\n Door." }
        };
    }

    // Update is called once per frame
    void Update()
    {
        numSnowballsText.text = numSnowballs.ToString();
        levelText.text = level.ToString();
    }

    public void SetResultString()
    {
        if(level < 5)
        {
            resultMessage.text = levelToResult[level];
        }
        else
        {
            resultMessage.text = "You got through " + level.ToString() + " doors";
        }
    }
}
