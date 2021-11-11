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

    // Start is called before the first frame update
    void Start()
    {
        numSnowballsText.text = "0";
        levelText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        numSnowballsText.text = numSnowballs.ToString();
        levelText.text = level.ToString();
    }
}
