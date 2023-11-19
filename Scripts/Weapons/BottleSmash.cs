using UnityEngine;

public class BottleSmash : MonoBehaviour
{
    private AudioSource audioPlayer;
    private Rigidbody rb;
    private bool playSound = false;
    private float destroyTime = 0.5f;

    public bool flames = false;
    public GameObject explosion;
    public GameObject bottleParent;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        Destroy(bottleParent, 15);                  //Fix bottle not destroy when not collide with anything
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!playSound)             //Fix sound bug
        {
            playSound = true;
            audioPlayer.Play();
            rb.isKinematic = true;
            SaveScripts.bottlePos = this.transform.position;
            Destroy(bottleParent, destroyTime);
        }

        if (flames)             //Mean that the bottle is the molotov
        {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
        }
    }
}
