using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;






public class pickupScript : MonoBehaviour
{
   [Header("Ints")]
    public int coins;

    public int stars;
    public int lives;
    [Header("UI")]
    public Canvas canvas;

    public TextMeshProUGUI _coinTextMeshProUgui;
    public TextMeshProUGUI _starTextMeshProUgui;
    public TextMeshProUGUI _MarioLivesTextMeshProUgui;

    public String[] spriteArray;
    

    public GameObject Star;

    public Animator thisAnimator;



    //audio Control

    public AudioSource MarioAudio;
    public AudioClip coinPickUpClip;
    public AudioClip starPickUpClip;
    public AudioClip deathClip;
   
    // Start is called before the first frame update
    void Start()
    {
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
        if (other.gameObject.CompareTag("star"))
        {
            stars = stars + 1;
            Debug.Log(stars);
            other.gameObject.SetActive(false);
            MarioAudio.PlayOneShot(starPickUpClip);




            if (other.gameObject.CompareTag("coin"))
            {
                coins = coins + 1;
                Debug.Log(coins);
                other.gameObject.SetActive(false);
                //coinPickUpSound = true;
                MarioAudio.PlayOneShot(coinPickUpClip);



            }


            if (other.gameObject.CompareTag("Killzone"))
            {
                lives = lives - 1;
                if (lives == 0)
                {
                    GetComponent<MasterMovement>().speed = 0f;
                    MarioAudio.PlayOneShot(deathClip, 0.8f);

                    //Let audio have enough time to play
                    Invoke("RestartScene", 4f);

                }

             }
         }
    }




    void updateCoins()
    {

        _coinTextMeshProUgui.text = coins.ToString();

        if (coins>=10)
        {
        }

       // _coinTextMeshProUgui.text = coins.ToString();

    }

    void updateStars()
    {

        _starTextMeshProUgui.text =stars.ToString();

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


    //array holds sprite draw strings for TMPro
    /* void setArray()
     {
         spriteArray[0] = "<sprite index = 0>";
         spriteArray[1] = "<sprite index = 1>";
         spriteArray[2] = "<sprite index = 2>";
         spriteArray[3] = "<sprite index = 3>";
         spriteArray[4] = "<sprite index = 4>";
         spriteArray[5] = "<sprite index = 5>";
         spriteArray[6] = "<sprite index = 6>";
         spriteArray[7] = "<sprite index = 7>";
         spriteArray[8] = "<sprite index = 8>";
         spriteArray[9] = "<sprite index = 9>";
     }
 //Numbers above 9 adds sprite combinations to cover all numbers
   /*  void numbersAbove9()
     {
         if (stars == 10)
         {
             _starTextMeshProUgui.text = spriteArray[1] + spriteArray[9];
         }

         if (coins == 10)
         {
             _coinTextMeshProUgui.text = spriteArray[1] + spriteArray[9];
         }
         if (stars == 11)
         {
             _starTextMeshProUgui.text = spriteArray[0] + spriteArray[0];
         }

         if (coins == 11)
         {
             _coinTextMeshProUgui.text = spriteArray[1] + spriteArray[1];
         }

         if (coins == 12)
         {
             _coinTextMeshProUgui.text = spriteArray[0] + spriteArray[1];
         }
     }
 */



}
