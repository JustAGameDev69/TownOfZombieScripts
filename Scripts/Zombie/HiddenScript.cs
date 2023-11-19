using UnityEngine;

public class HiddenScript : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHide"))
            SaveScripts.isHidden = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerHide"))
            SaveScripts.isHidden = false;
    }
}
