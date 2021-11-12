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
            {0, "You lost on the first door.\n That is unlucky." },
            {1, "You only got through 1 door.\n You are simply a dork." },
            {2, "You survived 2 doors.\n You're a knock-knock joke." },
            {3, "You got through 3 doors.\n How adorable." },
            {4, "You got through 4 doors.\n All your life has been a series of doors in your face." },
            {5, "You got through 5 doors.\n You're a doorbell." },
            {6, "You made it through 6 doors.\n Some people stay far away from the door,\n if there's a chance of it opening up.\n So should you." },
            {7, "You got though the 7th one?\n Shut the front door." },
            {8, "You survived 8 doors,\n but now you're a doornail." },
            {9, "You got through 9 doors.\n You basically live on Dorset Street." },
            {10, "You got through 10 doors?\n, Okay, Dorothy." },
            {11, "11 doors.\n Welcome to El Dorado." },
            {12, "You got through 12 doors.\n That's good." }
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
        if(level < 13)
        {
            resultMessage.text = levelToResult[level];
        }
        else
        {
            resultMessage.text = "You got through " + level.ToString() + " doors.\n You're a sophisticated door-opener.";
        }
    }
}
