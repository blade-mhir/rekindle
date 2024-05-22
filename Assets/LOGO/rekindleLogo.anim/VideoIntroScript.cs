using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Add this line to include the SceneManager namespace
using UnityEngine.Video;  // Add this line to include the VideoPlayer namespace

public class VideoIntroScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "MainGame";
    
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    // This method is called when the video reaches its end
    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
