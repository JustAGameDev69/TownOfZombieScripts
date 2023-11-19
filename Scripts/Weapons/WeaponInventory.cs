using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    [Header("Weapons Icon")]
    public Sprite[] bigIcons;
    public Image bigIcon;

    [Header("Weapons Title")]
    public string[] titles;
    public Text title;

    [Header("Weapons Description")]
    public string[] descriptions;
    public Text description;
    public Text weaponAmountText;

    [Header("Other")]
    public Button[] weaponButton;
    public GameObject sprayPanel;

    [Header("Combine Item")]
    [SerializeField] public GameObject useButton;
    public GameObject combineButton;
    public GameObject combinePanel;
    public GameObject combineUseButton;
    public Image[] combineItem;

    [Header("Audio")]
    public AudioClip click, select;
    private AudioSource audioPlayer;

    private int chosenWeaponNumber;

    private void OnEnable()
    {
        CheckForWeaponPickup();

        for (int i = 0; i < combineItem.Length; i++)
        {
            if (SaveScripts.itemPickup[i + 2])
                combineItem[i].color = Color.white;
            else
                combineItem[i].color = new Color(1, 1, 1, 0.06f);
        }

        if (SaveScripts.weaponAmount[chosenWeaponNumber] <= 0)
            ChosenWeapon(0);

        if (chosenWeaponNumber < 6)          //Only allow combine button with sparycan and bottle
            TurnOffCombineMenu();


    }

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        SetWeaponInventoryUI(0);                                //Defaul weapon is the knife

    }


    #region Equip Weapon
    public void ChosenWeapon(int weaponNumber)
    {
        TurnOffCombineMenu();

        SetWeaponInventoryUI(weaponNumber);
        audioPlayer.clip = click;
        audioPlayer.Play();
        weaponAmountText.text = "Amount: " + SaveScripts.weaponAmount[weaponNumber];

        if (chosenWeaponNumber == 6)
            useButton.SetActive(false);
        else
            useButton.SetActive(true);

        if (chosenWeaponNumber > 5)
            combineButton.SetActive(true);
    }

    private void SetWeaponInventoryUI(int weaponNumber)
    {
        chosenWeaponNumber = weaponNumber;
        bigIcon.sprite = bigIcons[weaponNumber];
        title.text = titles[weaponNumber];
        description.text = descriptions[weaponNumber];
    }

    public void AssignWeapon()
    {
        SaveScripts.weaponID = chosenWeaponNumber;
        audioPlayer.clip = select;
        audioPlayer.Play();
    }


    private void CheckForWeaponPickup()
    {
        for (int i = 0; i < weaponButton.Length; i++)
        {
            if (SaveScripts.weaponPickup[i] == false)
            {
                //If we haven't pick up these weapon -> cannot choose it.
                weaponButton[i].image.color = new Color(1, 1, 1, 0.06f);
                weaponButton[i].image.raycastTarget = false;
            }
            else if (SaveScripts.weaponPickup[i] == true)           //reverse
            {
                weaponButton[i].image.color = new Color(1, 1, 1, 1);
                weaponButton[i].image.raycastTarget = true;
            }
        }
    }
    #endregion

    public void CombineAction()
    {
        combinePanel.SetActive(true);

        if(chosenWeaponNumber == 6)
        {
            combineItem[1].transform.gameObject.SetActive(false);
            if (SaveScripts.itemPickup[2])
                combineUseButton.SetActive(true);
            else if (!SaveScripts.itemPickup[2])
                combineUseButton.SetActive(false);
        }
        else if(chosenWeaponNumber == 7)
        {
            combineItem[1].transform.gameObject.SetActive(true);
            if (SaveScripts.itemPickup[2]  && SaveScripts.itemPickup[3])
                combineUseButton.SetActive(true);
            else if (!SaveScripts.itemPickup[2] || !SaveScripts.itemPickup[3])
                combineUseButton.SetActive(false);
        }
    }
    private void TurnOffCombineMenu()
    {
        combinePanel.SetActive(false);
        combineButton.SetActive(false);
    }

    public void AssignCombineWeapon()
    {
        if(chosenWeaponNumber == 6)
        {
            SaveScripts.weaponID = chosenWeaponNumber;
            if(sprayPanel.GetComponent<SprayCanScript>().sprayAmount <= 0.0f)       //Refill sprayCan
                sprayPanel.GetComponent<SprayCanScript>().sprayAmount = 1.0f;
        }
        else if (chosenWeaponNumber == 7)
            SaveScripts.weaponID = chosenWeaponNumber += 1;

        audioPlayer.clip = select;
        audioPlayer.Play();
    }
}
