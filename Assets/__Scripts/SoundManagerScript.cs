using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip damagedSound, pointSound, powerUpSound, powerUpBadSound, shootSound, nukeSound;
    static AudioSource audioSrc;



    // Start is called before the first frame update
    void Start()
    {
        damagedSound = Resources.Load<AudioClip>("damaged");
        pointSound = Resources.Load<AudioClip>("point");
        powerUpSound = Resources.Load<AudioClip>("powerUp");
        powerUpBadSound = Resources.Load<AudioClip>("powerUpBad");
        shootSound = Resources.Load<AudioClip>("shoot");
        nukeSound = Resources.Load<AudioClip>("nuke");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void PlaySound(string clip)
    {
        switch (clip)
        {

            case "point":
                audioSrc.PlayOneShot(pointSound);
                break;
            case "powerUp":
                audioSrc.PlayOneShot(powerUpSound);
                break;
            case "powerUpBad":
                audioSrc.PlayOneShot(powerUpBadSound);
                break;
            case "shoot":
                audioSrc.PlayOneShot(shootSound);
                break;
            case "damaged":
                audioSrc.PlayOneShot(damagedSound);
                break;
            case "nuke":
                audioSrc.PlayOneShot(nukeSound);
                break;

        }

    }
}
