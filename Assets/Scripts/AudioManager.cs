using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{

    private AudioSource backgroundMusicSource; 

    void Start()
    {
        backgroundMusicSource = GetComponent<AudioSource>();
    }

    public void PauseMusic() 
    {
        backgroundMusicSource.Pause();
    }

    public void ResumeMusic()
    {
        backgroundMusicSource.UnPause(); 
    }

}
