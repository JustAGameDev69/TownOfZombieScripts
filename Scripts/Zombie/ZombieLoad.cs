using UnityEngine;

public class ZombieLoad : MonoBehaviour
{
    private void Start()
    {
        Invoke("SwitchOff", 1);
    }

    private void SwitchOff()
    {
        this.gameObject.SetActive(false);
    }

}
