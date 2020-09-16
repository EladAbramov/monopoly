using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject dice;
    public GameObject waypoints;
    public GameObject rewardText;
    public GameObject player1SumObj;
    public GameObject player2SumObj;
    public GameObject nowPlayingBar;
    public GameObject redBG;
    public GameObject blueBG;

    private int diceNum = 0;
    private bool isFirstPlayer = true;

    private string wayPoint = "Waypoint";
    private int currPlayer1Index = 0;
    private int currPlayer2Index = 0;

    private Dictionary<int, int> rewardsDict = new Dictionary<int, int>();
    private List<int> rewards = new List<int>();
    private int[] rewardsIndices = new int[4] { 0, 6, 12, 18 };

    private int player1Sum = 500;
    private int player2Sum = 500;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        rewardsDict.Add(50, 50);
        rewardsDict.Add(40, 10);
        rewardsDict.Add(60, 10);
        rewardsDict.Add(30, 10);
        rewardsDict.Add(70, 10);
        rewardsDict.Add(90, 5);
        rewardsDict.Add(10, 5);

        populateRewards();
    }

    private void populateRewards()
    {
        foreach (KeyValuePair<int, int> rewardPair in rewardsDict)
        {
            for (int i = 0; i < rewardPair.Value; i++)
            {
                rewards.Add(rewardPair.Key);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDice(int num)
    {
        diceNum = num;
        movePlayer(diceNum);
    }

    private void movePlayer(int num)
    {
        if (isGameOver)
        {
            return;
        }

        resetSign();

        GameObject currentPlayer;
        int currentIndex;
        if (isFirstPlayer)
        {
            currentPlayer = player1;
            currentIndex = currPlayer1Index;
        } else
        {
            currentPlayer = player2;
            currentIndex = currPlayer2Index;
        }

    
        currentIndex += num;
        if (currentIndex > 23)
        {
            currentIndex = currentIndex - 24;
        }

        checkReward(currentIndex);

        if (isFirstPlayer)
        {
            currPlayer1Index = currentIndex;
        } else
        {
            currPlayer2Index = currentIndex;
        }

        string targetString = wayPoint + currentIndex;


        Transform trans = waypoints.transform;
        Transform childTrans = trans.Find(targetString);

        currentPlayer.transform.position = childTrans.position;

        if (currentIndex != 0 && currentIndex != 6 && currentIndex != 12 && currentIndex != 18)
        {
            checkPurchace(childTrans);
        }

        isFirstPlayer = !isFirstPlayer;

        Transform bar = nowPlayingBar.GetComponent<Transform>();

        if (isFirstPlayer)
        {
            bar.localPosition = new Vector3(6, bar.localPosition.y, bar.localPosition.z);
        }
        else
        {
            bar.localPosition = new Vector3(454, bar.localPosition.y, bar.localPosition.z);
        }
    }

    private void checkPurchace(Transform childTrans)
    {
        if (childTrans.gameObject.GetComponent<waypointMgr>().isOwned)
        {
            if (childTrans.gameObject.GetComponent<waypointMgr>().isFirstPlayerOwned && !isFirstPlayer ||
                !childTrans.gameObject.GetComponent<waypointMgr>().isFirstPlayerOwned && isFirstPlayer)
            {
                // pay fine
                if (isFirstPlayer)
                {
                    player1Sum -= 150;
                    player1SumObj.GetComponent<Text>().text = player1Sum.ToString();
                    if (player1Sum < 0)
                    {
                        rewardText.GetComponent<Text>().text = "PLAYER 1 LOST!!";
                        isGameOver = true;
                    }
                } else
                {
                    player2Sum -= 150;
                    player2SumObj.GetComponent<Text>().text = player2Sum.ToString();
                    if (player2Sum < 0)
                    {
                        rewardText.GetComponent<Text>().text = "PLAYER 2 LOST!!";
                        Time.timeScale = 0f;
                        isGameOver = true;
                    }
                }
            }
        } else
        {
            // purchace if possible
            if (isFirstPlayer)
            {
                if (player1Sum >= 250)
                {
                    player1Sum -= 250;
                    GameObject redBg = Instantiate(redBG, childTrans.position, childTrans.rotation);
                    redBg.transform.SetParent(childTrans);
                    redBg.transform.localScale = new Vector3(50, 50, 1);
                    childTrans.gameObject.GetComponent<waypointMgr>().isOwned = true;
                    childTrans.gameObject.GetComponent<waypointMgr>().isFirstPlayerOwned = true;
                }
            } else
            {
                if (player2Sum >= 250)
                {
                    player2Sum -= 250;
                    GameObject blueBg = Instantiate(blueBG, childTrans.position, childTrans.rotation);
                    blueBg.transform.SetParent(childTrans);
                    blueBg.transform.localScale = new Vector3(50, 50, 1);
                    childTrans.gameObject.GetComponent<waypointMgr>().isOwned = true;
                    childTrans.gameObject.GetComponent<waypointMgr>().isFirstPlayerOwned = false;
                }

            }
        }
        player1SumObj.GetComponent<Text>().text = player1Sum.ToString();
        player2SumObj.GetComponent<Text>().text = player2Sum.ToString();
    }

    private void checkReward(int playerIndex)
    {
        bool isOnReward = false;

        foreach (int rewardIndex in rewardsIndices)
        {
            if (rewardIndex == playerIndex)
            {
                isOnReward = true;
            }
        }

        string playerNum = "";
        int randomRewardIndex = 0;
        if (isOnReward)
        {
            randomRewardIndex = Random.Range(0,100);

            if (isFirstPlayer)
            {
                player1Sum += rewards[randomRewardIndex];
                player1SumObj.GetComponent<Text>().text = player1Sum.ToString();
                playerNum = " 1 ";
            } else
            {
                player2Sum += rewards[randomRewardIndex];
                player2SumObj.GetComponent<Text>().text = player2Sum.ToString();
                playerNum = " 2 ";
            }

            rewardText.GetComponent<Text>().text = "Player" + playerNum + "won a reward of " + rewards[randomRewardIndex].ToString();
        }


        isOnReward = false;
    }

    private void resetSign()
    {
        rewardText.GetComponent<Text>().text = "";
    }

    public void restartScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
