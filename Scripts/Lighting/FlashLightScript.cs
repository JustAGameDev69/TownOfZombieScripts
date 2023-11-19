using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightScript : MonoBehaviour
{
    private Image flBattteryChunks;
    public float batteryPower = 1.0f;
    [SerializeField] private float drainTime = 90;

    private void OnEnable()
    {
        flBattteryChunks = GameObject.Find("FlBatteryChunks").GetComponent<Image>();
        InvokeRepeating("FLBatteryDrain", drainTime, drainTime);
    }

    private void FLBatteryDrain()
    {
        if (batteryPower >= 0.0f)
        {
            batteryPower -= 0.25f;
            UpdateBatteryFillAmount();
        }

    }

    public void UpdateBatteryFillAmount() => flBattteryChunks.fillAmount = batteryPower;

    public void StopDrain() => CancelInvoke("FLBatteryDrain");
}
