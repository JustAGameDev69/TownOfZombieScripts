using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void OnCLickMenuButton()
    {
        SceneManager.LoadScene(0);
    }

}
