using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Teams { Aliens, Security }

public class GameManager : MonoBehaviour {

    public PlayerManager playerManager;
    public Transform[] spawnPositions;

    public GameObject securityWinPanel;
    public GameObject aliensWinPanel;

    private float gameEndTime;
    private bool gameInProgress;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {

        // Reset game when they press R AND game is over
        if (Input.GetKeyDown("r") && !gameInProgress)
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
        gameInProgress = false;
    }


    public void ResetGame()
    {
        for (int i = 0; i < playerManager.playerControllers.Count; i++)
        {
            playerManager.playerControllers[i].transform.position = spawnPositions[i].position;
            playerManager.playerControllers[i].ResetHealth();

            // Because I don't want to check which panel is active
            aliensWinPanel.SetActive(false);
            securityWinPanel.SetActive(false);
        }
    }
}
