using UnityEngine;

public class InventorySwitching : MonoBehaviour
{
    [SerializeField] private GameObject itemsMenu;
    [SerializeField] private GameObject weaponsMenu;
    [SerializeField] private GameObject combinePanel;

    private void Start()
    {
        SwitchWeaponMenu();
    }

    public void SwitchItemMenu()
    {
        weaponsMenu.SetActive(false);
        itemsMenu.SetActive(true);
        combinePanel.SetActive(false);
    }

    public void SwitchWeaponMenu()
    {
        weaponsMenu.SetActive(true);
        itemsMenu.SetActive(false);
    }
}
