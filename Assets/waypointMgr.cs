using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointMgr : MonoBehaviour
{
    public bool isOwned = false;
    public bool isFirstPlayerOwned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOwned(bool isFirstPlayer)
    {
        isOwned = true;
        isFirstPlayerOwned = isFirstPlayer;
    }
}
