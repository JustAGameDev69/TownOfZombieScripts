using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pistolPanel, shotgunPanel, sprayPanel;
    [SerializeField] private Text pistolAmmo, pistolTotalAmmo, shotgunAmmo, shotgunTotalAmmo;

    private bool isPanelOn = false;

    private void Update()
    {
        if(SaveScripts.weaponID == 4 && !isPanelOn)
        {
            isPanelOn = true;
            pistolPanel.SetActive(true);
        }
        else if(SaveScripts.weaponID == 5 && !isPanelOn)
        {
            isPanelOn = true;
            shotgunPanel.SetActive(true);
        }
        else if (SaveScripts.weaponID == 6 && !isPanelOn)
        {
            isPanelOn = true;
            sprayPanel.SetActive(true);
        }

        if (SaveScripts.inventoryOpen)
        {
            pistolPanel.SetActive(false);
            shotgunPanel.SetActive(false);
            sprayPanel.SetActive(false);
            isPanelOn = false;
        }
    }

    private void OnGUI()
    {
        pistolTotalAmmo.text = SaveScripts.ammoAmount[0].ToString();
        pistolAmmo.text = SaveScripts.currentAmmo[4].ToString();
        shotgunTotalAmmo.text = SaveScripts.ammoAmount[1].ToString();
        shotgunAmmo.text = SaveScripts.currentAmmo[5].ToString();

    }
}
