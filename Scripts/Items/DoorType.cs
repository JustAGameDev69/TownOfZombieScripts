using UnityEngine;

public class DoorType : MonoBehaviour
{
    public enum TypeOfDoor
    {
        cabinet,
        house,
        cabin
    }

    public TypeOfDoor typeOfDoor;

    public bool opened = false;
    public bool locked = false;
    public bool electricDoor = false;

    
    [HideInInspector] public string message = "Press E to open";
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(opened == true)
        {
            animator.SetTrigger("Open");
            message = "Press E to close";
        }
    }
}
