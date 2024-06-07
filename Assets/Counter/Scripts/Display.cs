using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Display : MonoBehaviour
{
    public TextMeshProUGUI CounterText;
    public TextMeshProUGUI ScoreText;

    private int Count;
    private int Score;


    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boxCollided(int collideScore)
    {
        Count += 1;
        CounterText.text = "Count: " + Count;

        //if (other.tag == "Sphere")
        //{
        //    Score += 5;
        //}
        //else if (other.tag == "Cube")
        //{
        //    Score += 10;
        //}

        Score += collideScore;

        ScoreText.text = "Score: " + Score;
    }
}
