using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Counter : MonoBehaviour
{
    private Display displayScript;

    private void Start()
    {
        //find the spawnManager Object to get the Display script from
        displayScript = GameObject.Find("SpawnManager").GetComponent<Display>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sphere")
        {
            displayScript.boxCollided(5);
        }
        else if (other.tag == "Cube")
        {
            displayScript.boxCollided(10);
        }
    }
}
