using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioCarriere : MonoBehaviour
{


    public AudioSource marioSoundEffect;

    //Pick up script sound effect
    public AudioClip coinPickUpClip;
    public AudioClip starPickupClip;
    public AudioClip deathClip;

    // Start is called before the first frame update
    void Start()
    {
        marioSoundEffect = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pickupScript.starPickUpSound == true) {

            marioSoundEffect.PlayOneShot(starPickupClip);
         
           }

        if(pickupScript.coinPickUpSound == true) {

            marioSoundEffect.PlayOneShot(coinPickUpClip);
        }
       

        if (pickupScript.deathSound == true) {

            marioSoundEffect.PlayOneShot(deathClip);
          }
    }
}
