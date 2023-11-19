using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LookMode : MonoBehaviour
{
    private Camera cam;

    [Header("Post Process")]
    public PostProcessProfile standard;
    public PostProcessProfile nightVision;
    public PostProcessProfile inventory;
    private PostProcessVolume vol;

    [Header("Night Vision")]
    public GameObject nightVisionOverlay;
    private bool isNightVisionOn = false;
    private NightVisionScript nightVisionScript;

    [Header("Flash Light")]
    public GameObject flashLightOverlay;
    private bool isflashLightOn = false;
    private Light flashLight;
    private FlashLightScript flashLightScript;

    [Header("Inventory")]
    public GameObject inventoryOverlay;
    [SerializeField] private GameObject combinePanel;
    private bool isInventoryOn;

    private bool isFLBatteryOver = false;
    private bool isNVBatteryOver = false;
    [SerializeField] private GameObject centerPointer;

    private void Start()
    {
        flashLightScript = flashLightOverlay.GetComponent<FlashLightScript>();
        nightVisionScript = nightVisionOverlay.GetComponent<NightVisionScript>();
        cam = GetComponent<Camera>();
        flashLight = GetComponentInChildren<Light>();
        vol = GetComponent<PostProcessVolume>();
        nightVisionOverlay.SetActive(false);
        flashLightOverlay.SetActive(false);
        vol.profile = standard;
    }

    private void Update()
    {
        NightVisionControl();

        FlashLightControl();

        if (Input.GetKeyDown(KeyCode.I) && Time.timeScale == 1.0f)
        {
            if (!SaveScripts.inventoryOpen && !isInventoryOn)
            {
                if (isflashLightOn)
                    TurnOffFlashLight();
                if (isNightVisionOn)
                    TurnOffNightVision();

                Cursor.visible = true;
                combinePanel.SetActive(false);
                inventoryOverlay.SetActive(true);
                vol.profile = inventory;
                isInventoryOn = true;
            }
            else if (SaveScripts.inventoryOpen && isInventoryOn)
            {
                inventoryOverlay.SetActive(false);
                vol.profile = standard;
                isInventoryOn = false;
                Cursor.visible = false;
            }
        }

    }

    #region Flash Light
    private void FlashLightControl()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFLBatteryOver && !isInventoryOn)
        {
            if (isflashLightOn == false)
            {
                flashLightOverlay.SetActive(true);
                flashLight.enabled = true;
                isflashLightOn = true;
            }
            else if (isflashLightOn == true)
            {
                TurnOffFlashLight();
            }
        }

        if(flashLightScript.batteryPower <= 0)
        {
            TurnOffFlashLight();
            isFLBatteryOver = true;
        }
    }

    private void TurnOffFlashLight()
    {
        flashLightScript.StopDrain();
        flashLightOverlay.SetActive(false);
        flashLight.enabled = false;
        isflashLightOn = false;
    }

    #endregion

    #region Night Vision Glass

    private void NightVisionControl()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (isNightVisionOn == false && !isNVBatteryOver && !isInventoryOn)
            {
                vol.profile = nightVision;
                nightVisionOverlay.SetActive(true);
                isNightVisionOn = true;
            }
            else if (isNightVisionOn == true)
            {
                TurnOffNightVision();
            }

        }

        if (nightVisionScript.batteryPower <= 0)
        {
            TurnOffNightVision();
            isNVBatteryOver = true;
        }

    }

    public void TurnOffNightVision()
    {
        nightVisionScript.StopDrain();
        cam.fieldOfView = 60;
        vol.profile = standard;
        nightVisionOverlay.SetActive(false);
        isNightVisionOn = false;
    }

    #endregion
}
