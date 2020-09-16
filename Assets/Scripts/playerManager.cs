using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour {
    public GameObject dice;
    private int num;
    private diceManager diceNumber;
    public GameObject firstSlot;

    [SerializeField]
    private float speed = 1f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false; 

    private void Start()
    {
        diceNumber = dice.GetComponent<diceManager>();
        transform.position = firstSlot.transform.position;
    }
}
