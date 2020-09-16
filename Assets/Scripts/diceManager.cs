using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceManager : MonoBehaviour
{
    public int finalSide;
    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;

    public GameObject gameManagerObj;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        
    }

  
    private void OnMouseDown()
    {
        if (gameManagerObj.GetComponent<GameManager>().isGameOver)
        {
            return;
        }
        rollTheDice();
    }

    // Coroutine that rolls the dice
    private void rollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration

            //yield WaitForSeconds(0.05f);
            //StartCoroutine(Wait(0.5f));
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;
        // Show final dice value in Console
        //Debug.Log(finalSide);

        gameManagerObj.GetComponent<GameManager>().setDice(finalSide);
    }

    private IEnumerator Wait(float seconds)
    {
        Debug.Log("start waiting");
        yield return new WaitForSeconds(seconds);
        Debug.Log("waiting");
    }
}
