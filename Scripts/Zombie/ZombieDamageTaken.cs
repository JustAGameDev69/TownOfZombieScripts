using UnityEngine;

public class ZombieDamageTaken : MonoBehaviour
{
    [SerializeField] private GameObject zombieDamageObject;
    private Animator animator;

    public GameObject[] lods;
    public GameObject flame;
    public Material skinBurn;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SendGunDamage(Vector3 hitPoint)
    {
        zombieDamageObject.GetComponent<ZombieDamage>().gunDamage(hitPoint);
    }

    private void OnParticleCollision(GameObject other)
    {
        zombieDamageObject.GetComponent<ZombieDamage>().FlameDeath();
        flame.SetActive(true);
        foreach (GameObject lod in lods)
        {
            lod.GetComponent<Renderer>().material = skinBurn;
        }
        animator.SetTrigger("burn");
    }
}
