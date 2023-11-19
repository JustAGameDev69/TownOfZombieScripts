using UnityEngine;

public class BeginScript : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
    }

    public void BeginButton()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Destroy(gameObject);
    }

}
