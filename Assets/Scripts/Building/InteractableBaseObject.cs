using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableBaseObject : MonoBehaviour
{
    [SerializeField]
    int objectType = 0;
    [SerializeField]
    Canvas options;
    [SerializeField]
    bool isDisplaying = false; //used to help search through arrays and enable/disable ui elements

    void Start()
    {
        options.enabled = false;
    }

    public void AltInteraction() //add to this sript for different interactables. 0 = door, 1 = ladder/vine, etc.
    {
        switch(objectType)
        {
            case 0:
                DisplayDoor();
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    void DisplayDoor()
    {
        isDisplaying = true;
        Debug.Log("Door info displayed");
        options.enabled = true;
    }

    public void HideElements()
    {
        options.enabled = false;
        isDisplaying = false;
        Debug.Log("Hidden UI for object");
    }

    void Update()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) && isDisplaying)
            {
                HideElements();
            }
        }
    }

    void Erase()
    {
        //remove from counters and stuff
        Destroy(this.gameObject);
    }
}
