using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.SceneManagement;

public abstract class VoteBase : NetworkBehaviour
{
    [Header("=VOTE BASE SETTINGS=")]
    [SyncVar] public float voteTimerMax = 20.0f;
    [SyncVar] public float voteTimer;
    public bool isPlayerVoted = false;

    public abstract void CountDownTimer();

    [Server]
    public virtual bool ChooseMostVotedInBool(int firstValue, int secondValue)
    {
        if (firstValue > secondValue)
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }

    [Server]
    public virtual int ChooseMostVotedElementInList(List<int> list)
    {
        int biggestValue = 0;
        int mostVotedElementID = 0;

        biggestValue = list.Max();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == biggestValue)
            {
                mostVotedElementID = i;
            }
        }

        return mostVotedElementID;
    }
    
    [Server]
    public virtual void LoadChoosenMap(string sceneName)
    {
        NetworkManagerVote.instance.ServerChangeScene(sceneName);
    }

    [Server]
    public virtual void ReplayGame()
    {
        Debug.Log("Replaying Game...");
        NetworkManagerVote.instance.ServerChangeScene(SceneManager.GetActiveScene().name);
    }
}
