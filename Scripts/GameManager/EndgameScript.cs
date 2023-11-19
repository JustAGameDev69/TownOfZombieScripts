using UnityEngine;

public class EndgameScript : MonoBehaviour
{
    public GameObject winMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SaveScripts.gotVaccine)
            {
                Cursor.visible = true;
                winMessage.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
