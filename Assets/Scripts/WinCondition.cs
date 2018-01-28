using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Teams { Aliens, Security }

public class WinCondition : MonoBehaviour {

    private PlayerManager playerManager;

    public GameObject securityWinPanel;
    public GameObject aliensWinPanel;

    private float gameEndTime;
    private bool gameInProgress;

    // Use this for initialization
    void Start () {
        SetWinner(Teams.Aliens);
	}
	
	// Update is called once per frame
	void Update () {
		
        // Reset when we have a game that has ended and 5 seconds have passed
        if (!gameInProgress && Time.time > gameEndTime + 5f)
        {
            gameInProgress = true;
            ResetGame();
        }
	}

    
    public void SetWinner(Teams winningTeam)
    {
        if (winningTeam == Teams.Aliens)
        {
            aliensWinPanel.SetActive(true);
        }
        else if (winningTeam == Teams.Security)
        {
            securityWinPanel.SetActive(true);
        }

        gameEndTime = Time.time;
    }


    public void ResetGame()
    {
        foreach (PlayerController player in playerManager.playerControllers)
        {
        }
    }
}
