using System.Collections;
using UnityEngine;

public class ZombieDamage : MonoBehaviour
{
    public string[] weaponTags;
    public int[] damageAmount;
    public AudioClip[] damageSound;

    private bool damaging = true;
    private bool isDead = false;
    private bool flameDeath = false;
    private int zombieHealth = 100;

    [SerializeField] private GameObject bloodSplat;
    private AudioSource damagePlayer;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        damagePlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            damaging = true;
        }

        if (zombieHealth <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("dead");
            animator.SetBool("attacking", false);
            animator.SetBool("isDead", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 pos = other.ClosestPoint(transform.position);
        for (int i = 0; i < weaponTags.Length; i++)
        {
            if (other.CompareTag(weaponTags[i]) && damaging)
            {
                damaging = false;
                zombieHealth -= damageAmount[i];
                Instantiate(bloodSplat, pos, other.transform.rotation);
                this.transform.gameObject.GetComponentInParent<ZombieScript>().isAngry = true;
                damagePlayer.clip = damageSound[i];
                damagePlayer.Play();
                if (weaponTags[i] == "bat")
                    animator.SetTrigger("react");
                if (weaponTags[i] == "axe")
                    animator.SetTrigger("axeReact");
            }
        }
    }

    public void gunDamage(Vector3 hitPoint)
    {
        zombieHealth -= 100;
        if (!isDead)
        {
            isDead = false;
            Instantiate(bloodSplat, hitPoint, this.transform.rotation);
            animator.SetTrigger("dead");
            animator.SetBool("isDead", true);
        }
    }

    public void FlameDeath()
    {
        if (!flameDeath)
        {
            flameDeath = true;
            StartCoroutine(ZombieOnFire());
        }
    }

    IEnumerator ZombieOnFire()
    {
        yield return new WaitForSeconds(8);
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("fireDie");
            animator.SetBool("isDead", true);
        }
    }

}
