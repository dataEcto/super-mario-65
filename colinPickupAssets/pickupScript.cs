﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pickupScript : MonoBehaviour
{
    public int coins;

    public int stars;
    public Canvas canvas;
    public TextMeshProUGUI _coinTextMeshProUgui;
    public TextMeshProUGUI _starTextMeshProUgui;

    public AudioClip starCollectsound;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        updateCoins();
        updateStars();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "star")
        {
            stars = stars + 1;
            other.gameObject.SetActive(false);
            
            
        }

        if (other.gameObject.tag == "coins")
        {
            coins = coins + 1;
            other.gameObject.SetActive(false);
            
        }
    }

    void updateCoins()
    {
        _coinTextMeshProUgui.text = coins.ToString();
    }

    void updateStars()
    {
        _starTextMeshProUgui.text = stars.ToString();
    }
}
