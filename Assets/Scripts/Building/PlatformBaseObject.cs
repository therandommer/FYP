using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBaseObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Erase()
    {
        //erase from arrays and whatnot
        Destroy(this.gameObject);
    }
}
