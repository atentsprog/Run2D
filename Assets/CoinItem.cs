﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Coin Enter:" + collision.transform);
        if (collision.transform.GetComponent<Player>() == null)
            return;

        GetComponentInChildren<Animator>().Play("HideCoin", 1);

        RunGameManager.instance.Coin += 100;
    }
}