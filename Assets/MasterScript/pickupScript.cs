using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;






public class pickupScript : MonoBehaviour
{
    [Header("Ints")] public int coins;

    public int stars;
    public int lives;
    [Header("UI")] public Canvas canvas;

    public TextMeshProUGUI _coinTextMeshProUgui;
    public TextMeshProUGUI _starTextMeshProUgui;
    public TextMeshProUGUI _MarioLivesTextMeshProUgui;

    public GameObject Star;

    public Animator thisAnimator;

    public Vector3 spawnPos;



    //audio Control

    public AudioSource MarioAudio;
    public AudioClip coinPickUpClip;
    public AudioClip starPickUpClip;
    public AudioClip deathClip;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = this.transform.position;
        //setArray();
        lives = 3;

        //audio static boolean, will not effect script

        MarioAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCoins();
        updateStars();
        updateLives();
    }

    // Triggers for coins, stars, and killzones
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something");
        
        if (other.gameObject.CompareTag("star"))
        {
            stars = stars + 1;
            Debug.Log(stars);
            other.gameObject.SetActive(false);
            MarioAudio.PlayOneShot(starPickUpClip);
            SceneManager.LoadScene(2);

        }

        if (other.gameObject.CompareTag("coin"))
        {
            coins = coins + 1;
            Debug.Log(coins);
            other.gameObject.SetActive(false);
            //MarioAudio.PlayOneShot(coinPickUpClip);

        }


        if (other.gameObject.CompareTag("killbox"))
        {
            lives = lives - 1;
            this.gameObject.transform.position += spawnPos;
            if (lives == 0)
            {
                GetComponent<MasterMovement>().speed = 0f;
                MarioAudio.PlayOneShot(deathClip, 0.8f);

                //Let audio have enough time to play
                Invoke("RestartScene", 4f);

            }

        }

    }




    void updateCoins()
    {

        _coinTextMeshProUgui.text = coins.ToString();

        if (coins >= 10)
        {
        }

        _coinTextMeshProUgui.text = coins.ToString();

    }

    void updateStars()
    {

        _starTextMeshProUgui.text = stars.ToString();

    }

    void updateLives()
    {
        _MarioLivesTextMeshProUgui.text = lives.ToString();


        //  _starTextMeshProUgui.text = stars.ToString();

    }

    void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

}

