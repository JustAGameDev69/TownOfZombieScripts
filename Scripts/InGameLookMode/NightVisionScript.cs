using UnityEngine;
using UnityEngine.UI;

public class NightVisionScript : MonoBehaviour
{
    private Image zoomBar;
    private Camera cam;
    private Image batteryChunk;

    public float batteryPower = 1.0f;
    public float drainTime = 60;

    private void OnEnable()
    {
        InvokeRepeating("NVBatteryDrain", drainTime, drainTime);

        if (zoomBar != null)
            zoomBar.fillAmount = 0.6f;
    }

    private void Start()
    {
        batteryChunk = GameObject.Find("BatteryChunks").GetComponent<Image>();
        zoomBar = GameObject.Find("ZoomBar").GetComponent<Image>();
        cam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && cam.fieldOfView >= 15)
        {
            cam.fieldOfView -= 5;
            zoomBar.fillAmount = cam.fieldOfView/100;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && cam.fieldOfView < 60)
        {
            cam.fieldOfView += 5;
            zoomBar.fillAmount = cam.fieldOfView / 100;
        }

    }

    private void NVBatteryDrain()
    {
        if (batteryPower > 0.0f)
        {
            batteryPower -= 0.25f;
            UpdateBatteryFillAmount();
        }
    }

    public void UpdateBatteryFillAmount()
    {
        batteryChunk.fillAmount = batteryPower;
    }

    public void StopDrain()
    {
        CancelInvoke("NVBatteryDrain");
    }

}
