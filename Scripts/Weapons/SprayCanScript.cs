using UnityEngine;
using UnityEngine.UI;

public class SprayCanScript : MonoBehaviour
{
    public Image sprayFill;
    public float sprayAmount = 1.0f;
    [SerializeField] private float drainTime;

    private void OnEnable()
    {
        sprayFill.fillAmount = sprayAmount;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            sprayAmount -= drainTime * Time.deltaTime;
            sprayFill.fillAmount = sprayAmount;
        }
    }
}
