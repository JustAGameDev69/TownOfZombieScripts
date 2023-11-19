using UnityEngine;

public class BreathingScript : MonoBehaviour
{
    private AudioSource audioPlayer;
    private bool isHeavyBreathing = false;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(SaveScripts.energy < 20 && !isHeavyBreathing)
        {
            isHeavyBreathing = true;
            audioPlayer.Play();
        }
        else if(SaveScripts.energy > 19)
        {
            isHeavyBreathing = false;
            audioPlayer.Stop();
        }
    }
}
