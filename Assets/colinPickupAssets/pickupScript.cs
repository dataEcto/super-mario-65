using System.Collections;
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

    public GameObject Star;

    public Animator thisAnimator;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("star"))
        {
            stars = stars + 1;
            Debug.Log(stars);
            other.gameObject.SetActive(false);
            
            
        }

        if (other.gameObject.CompareTag("coin"))
        {
            coins = coins + 1;
            Debug.Log(coins);
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



    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "star" && other.isTrigger == true) {

            thisAnimator.Play("StarPickedUp");
            Invoke("StarDisappear", 1.3f);

           
             }
    }



    void StarDisappear() {

        Star.SetActive(false);
      
       
         }
}
