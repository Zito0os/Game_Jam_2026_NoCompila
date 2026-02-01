using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineSceneLoader : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        if (director.state != PlayState.Playing)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}


