using UnityEngine;

public class UIScale : MonoBehaviour
{
    [SerializeField] private float scaleValue = 1f;

    private void Start()
    {
        if (Screen.width > 1920)
            scaleValue = 2f;

        this.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
}
