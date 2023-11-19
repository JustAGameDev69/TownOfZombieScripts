using UnityEngine;
using UnityEngine.UI;

public class MainGUI : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Text healthAmount;
    [SerializeField] private Text energyAmount;
    [SerializeField] private Text infectionAmount;

    private void Update()
    {
        healthAmount.text = SaveScripts.health + "%";
        energyAmount.text = SaveScripts.energy.ToString("F0") + "%";
        infectionAmount.text = SaveScripts.infection.ToString("F0") + "%";
    }
}
