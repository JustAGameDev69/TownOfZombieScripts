using UnityEngine;
using UnityEngine.UI;

public class ItemsInventory : MonoBehaviour
{
    public Sprite[] bigIcons;
    public string[] titles;
    public string[] descriptions;
    public Button[] itemButton;
    public Text title;
    public Text itemAmounText;
    public Image bigIcon;
    public Text description;
    public AudioClip click, select;
    public GameObject useButton;
    public GameObject flashLightPanel;
    public GameObject nightVisionPanel;
    public GameObject eletricDoor;
    public GameObject eletricLight1, eletricLight2;

    private AudioSource audioPlayer;
    private int chosenItemNumber;
    private int updateHealth;
    private float updateEnergy;
    private float updateInfection;
    private bool addHealth = false;
    private bool addEnergy = false;
    private bool reduceInfection = false;
    private bool flBatteryRefill = false;
    private bool nvBatteryRefill = false;


    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        SetItemInventoryUI(0);                                //Defaul weapon is the knife
        useButton.SetActive(false);
    }

    private void OnEnable()
    {
        SetInteractiveItems();

        if (SaveScripts.itemAmount[chosenItemNumber] <= 0)
            ChosenItems(0);

        ChosenItems(chosenItemNumber);

    }

    private void Update()
    {
        SetInteractiveItems();              //Fix use unlimited item

    }


    #region Set Items
    private void SetInteractiveItems()
    {
        for (int i = 0; i < itemButton.Length; i++)
        {
            if (SaveScripts.itemPickup[i] == false)
            {
                //If we haven't pick up these weapon -> cannot choose it.
                itemButton[i].image.color = new Color(1, 1, 1, 0.06f);
                itemButton[i].image.raycastTarget = false;
            }
            else if (SaveScripts.itemPickup[i] == true)           //reverse
            {
                itemButton[i].image.color = new Color(1, 1, 1, 1);
                itemButton[i].image.raycastTarget = true;
            }
        }
    }

    public void ChosenItems(int itemNumber)
    {
        //We only want use button appear with some items
        if(itemNumber < 4)
            useButton.SetActive(false);
        else
            useButton.SetActive(true);

        SetItemInventoryUI(itemNumber);
        if(audioPlayer != null)
        {
            audioPlayer.clip = click;
            audioPlayer.Play();
        }
        itemAmounText.text = "Amount: " + SaveScripts.itemAmount[itemNumber];

        if (itemNumber != 8)                        //Fix click on other item but flashlight auto refill
            flBatteryRefill = false;

        if (itemNumber != 9)                        //Also with the night vision battery
            nvBatteryRefill = false;
    }

    private void SetItemInventoryUI(int itemNumber)
    {
        chosenItemNumber = itemNumber;
        bigIcon.sprite = bigIcons[itemNumber];
        title.text = titles[itemNumber];
        description.text = descriptions[itemNumber];
    }
    public void AssignItem()            //Apply this for Use button
    {
        SaveScripts.itemID = chosenItemNumber;
        audioPlayer.clip = select;
        audioPlayer.Play();

        if (chosenItemNumber != 10 && chosenItemNumber != 11)                //House Key and Cabin key could be using forever
        {
            SaveScripts.itemAmount[chosenItemNumber]--;
            ChosenItems(chosenItemNumber);
            if (SaveScripts.itemAmount[chosenItemNumber] == 0)
            {
                SaveScripts.itemPickup[chosenItemNumber] = false;
                useButton.SetActive(false);
            }
        }

        UpdatePlayerStats();

        RefillBatteryForFLandNV();

        if(chosenItemNumber == 10)              //Using House Key
        {
            if (SaveScripts.doorObject != null)
            {
                if((int)SaveScripts.doorObject.GetComponent<DoorType>().typeOfDoor == 1 && SaveScripts.doorObject.GetComponent<DoorType>().locked == true)
                {
                    SaveScripts.doorObject.GetComponent<DoorType>().locked = false;
                }
            }
        }

        if (chosenItemNumber == 11)              //Using Cabin Key
        {
            if (SaveScripts.doorObject != null)
            {
                if ((int)SaveScripts.doorObject.GetComponent<DoorType>().typeOfDoor == 2 && SaveScripts.doorObject.GetComponent<DoorType>().locked == true)
                {
                    SaveScripts.doorObject.GetComponent<DoorType>().locked = false;
                }
            }
        }

        if(chosenItemNumber == 12)
        {
            if (SaveScripts.generator != null)
            {
                SaveScripts.generatorOn = true;
                SaveScripts.generator.GetComponent<AudioSource>().Play();

                eletricDoor.GetComponent<DoorType>().locked = false;
                eletricLight1.SetActive(true);
                eletricLight2.SetActive(true);
            }
        }

    }


    #endregion

    #region Update Player Stats
    private void UpdatePlayerStats()
    {
        if(addHealth)
        {
            addHealth = false;
            if (SaveScripts.health < 100)
                SaveScripts.health += updateHealth;
            if( SaveScripts.health > 100)
                SaveScripts.health = 100;
        }

        if (addEnergy)
        {
            addEnergy = false;
            if(SaveScripts.energy < 100)
                SaveScripts.energy += updateEnergy;
            if(SaveScripts.energy > 100)
                SaveScripts.energy = 100;
        }

        if (reduceInfection)
        {
            reduceInfection = false;
            if (SaveScripts.infection > 0.0f)
                SaveScripts.infection -= updateInfection;
            if (SaveScripts.infection < 0.0f)
                SaveScripts.infection = 0.0f;
        }
    }

    public void UpdateHealth(int _updateHealth)
    {
        updateHealth = _updateHealth;
        addHealth = true;
    }

    public void UpdateEnergy(int _updateEnergy)
    {
        updateEnergy = _updateEnergy;
        addEnergy = true;
    }

    public void UpdateInfection(int _updateInfection)
    {
        updateInfection = _updateInfection;
        reduceInfection = true;
    }

    #endregion

    #region Refill Battery
    private void RefillBatteryForFLandNV()
    {
        if (flBatteryRefill)
        {
            flBatteryRefill = false;
            flashLightPanel.GetComponent<FlashLightScript>().batteryPower = 1.0f;
            flashLightPanel.GetComponent<FlashLightScript>().UpdateBatteryFillAmount();
        }

        if (nvBatteryRefill)
        {
            nvBatteryRefill = false;
            nightVisionPanel.GetComponent<NightVisionScript>().batteryPower = 1.0f;
            nightVisionPanel.GetComponent<NightVisionScript>().UpdateBatteryFillAmount();
        }
    }

    public void RefillFlBattery()
    {
        flBatteryRefill = true;
    }

    public void RefillNVBattery()
    {
        nvBatteryRefill = true;
    }

    #endregion
}
