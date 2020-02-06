using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Cursor")
        {
            Debug.Log("Disabling cursor");
            other.SendMessage("EnablePlacement");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag=="Cursor")
        {
            Debug.Log("Enabling cursor");
            other.SendMessage("DisablePlacement");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
