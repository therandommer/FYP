using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Vector3 defaultPosition = new Vector3();
    [SerializeField]
    Player player;
    [SerializeField]
    GlobalController gc;
    [SerializeField]
    bool isGameplay = false;
    [SerializeField]
    float xOffset = 5.0f; //change this offset based on if the character is facing left or right, will need to lerp this
    [SerializeField]
    float yOffset = 4.0f;
    void Awake()
    {
        defaultPosition.x = this.transform.position.x;
        defaultPosition.y = this.transform.position.y;
        defaultPosition.z = this.transform.position.z;
        gc = FindObjectOfType<GlobalController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null && gc.GetIsGameplay())
        {
            player = FindObjectOfType<Player>();
        }
        if(gc.GetIsGameplay() && isGameplay == false)
        {
            isGameplay = true;
        }
        if(!gc.GetIsGameplay())
        {
            this.transform.position = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z);
            isGameplay = false;
        }
        if(isGameplay) //follows the payer location with a slight offset to allow for visibility
        {

        }
        else if(!isGameplay) //panning/button presses to change positions
        {

        }
    }
}
