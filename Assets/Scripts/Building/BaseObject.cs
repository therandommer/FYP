using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField] [Range(1,6)] //object type number is the same as buildsettings
    int objectType = 0;
    [SerializeField]
    int objectValue = 1;
    [SerializeField]
    Vector2 defaultPosition;
    [SerializeField]
    BuildSettings build;

    void Awake()
    {
		build = FindObjectOfType<BuildSettings>();
        if(build.GetCurrentObjects(objectType) + objectValue <= build.GetMaxObjects(objectType)) //used to limit and track number of objects placed. Mainly used to limit player to 1
        {
            build.IncrementObject(objectType, objectValue);
        }
        else
        {
            Erase();
        }
        defaultPosition = this.transform.position;
    }

    //will be called when player returns from gameplay to building
    private void Reset()
    {
        this.transform.position = defaultPosition;
    }
    void Erase()
    {
        //erase from arrays and whatnot
        Destroy(this.gameObject);
    }
}
