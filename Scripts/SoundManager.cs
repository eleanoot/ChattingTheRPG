using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager = null;
    public AudioSource loopSfx;
    public AudioSource singleSfx;
    public AudioSource bgSfx;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (soundManager == null)
            //if not, set it to this.
            soundManager = this;
        //If instance already exists:
        else if (soundManager != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void PlayLoopingSfx(AudioClip clip)
    {
        loopSfx.clip = clip;
        loopSfx.Play();
    }

    public void PlaySingleSfx(AudioClip clip)
    {
        singleSfx.clip = clip;
        singleSfx.Play();
    }
}
