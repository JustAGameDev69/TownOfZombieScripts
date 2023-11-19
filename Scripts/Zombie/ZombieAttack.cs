using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private bool canDamage = false;
    private Collider col;
    private Animator bloodEffect;
    private AudioSource hitSound;

    private void Start()
    {
        col = GetComponent<Collider>();
        bloodEffect = GameObject.Find("Blood").GetComponent<Animator>();
        hitSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!col.enabled)
        {
            canDamage = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canDamage)
            {
                canDamage = false;
                SaveScripts.health -= damageAmount;         //Currently this could go above limit, fix when Player die
                SaveScripts.infection += damageAmount;
                bloodEffect.SetTrigger("blood");
                hitSound.Play();
            }
        }
    }

}
