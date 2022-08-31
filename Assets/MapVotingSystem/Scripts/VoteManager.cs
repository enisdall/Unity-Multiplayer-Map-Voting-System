using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;
using System.Linq;

public class VoteManager : VoteBase
{   
    [Header("=POLL GENERATION=")]
    [SerializeField] private Map[] maps = null;
   // [SerializeField] private string[] gameModes = null;
    
    [Header("=POLLS=")]  
    [SerializeField] private List<string> Maps = new List<string>();
    [SerializeField] private List<int> Votes = new List<int>();

    SyncListString syncListMaps = new SyncListString();
    SyncListInt syncListVotes = new SyncListInt();

    public List<Button> MapPollButtons = new List<Button>();
  
    private void Awake()
    {  
        MapPollButtons[0] = GameObject.Find("MapPoll01").GetComponent<Button>();
        MapPollButtons[0].onClick.AddListener(() => DoVoteButton(0));

        MapPollButtons[1] = GameObject.Find("MapPoll02").GetComponent<Button>();
        MapPollButtons[1].onClick.AddListener(() => DoVoteButton(1));

        MapPollButtons[2] = GameObject.Find("MapPoll03").GetComponent<Button>();
        MapPollButtons[2].onClick.AddListener(() => DoVoteButton(2));

        MapPollButtons[3] = GameObject.Find("ReplayGame").GetComponent<Button>();
        MapPollButtons[3].onClick.AddListener(() => DoVoteButton(3));

        MapPollButtons[4] = GameObject.Find("RefreshPoll").GetComponent<Button>();
        MapPollButtons[4].onClick.AddListener(() => DoVoteButton(4));

        for (int i = 0; i < 5; i++)
        {
            Maps.Add("");
            Votes.Add(0);
            syncListVotes.Add(0);
            syncListMaps.Add("");
        }

        GeneratePolls();
        ResetTimer();
    }

    private void LateUpdate()
    {
        VisualizeSyncList();
        CountDownTimer();
    }

    #region Client
    [ClientCallback]
    public void DoVoteButton(int pollID)
    {
        if (!isPlayerVoted)
        {
            CmdDoVote(pollID);
           // isPlayerVoted = true;
        }
    }

    // Converts SyncList to our local 'Votes' list so we can make visible SyncList elements.
    [ClientCallback]
    void VisualizeSyncList()
    {
        for (int i = 0; i < syncListVotes.Count; i++)
        {
            Votes[i] = syncListVotes[i];
            Maps[i] = syncListMaps[i];
        }
    }
    #endregion

    #region Server
    [ServerCallback]
    void GeneratePolls()
    {
        for (int i = 0; i < syncListVotes.Count; i++)
        {
            syncListVotes[i] = 0;
        }

        for (int i = 0; i < syncListMaps.Count - 2; i++)
        {
            syncListMaps[i] = maps[Random.Range(0, maps.Length)].mName;
        }
    }

    [ServerCallback]
    public override void CountDownTimer()
    {
        if (voteTimer > 0)
        {
            voteTimer -= Time.deltaTime;
        }

        if (voteTimer <= 0)
        {
           int mostVotedElementID = ChooseMostVotedElementInList(Votes);
           VoteFinished(mostVotedElementID);
        }
    }

    [Command(ignoreAuthority = true)]
    void CmdDoVote(int pollID)
    {
        syncListVotes[pollID]++;
    }

    
    [Server]
    void VoteFinished(int mostVotedElementID)
    {       
        if (mostVotedElementID < 3)
        {
            base.LoadChoosenMap(Maps[mostVotedElementID]);
        }

        if (mostVotedElementID == 3)
        {
            base.ReplayGame();
        }

        if (mostVotedElementID == 4)
        {
            RefreshPoll();
        }      
    }

    
    [Server]
    void RefreshPoll()
    {
        Debug.Log("Refreshing Poll...");
        GeneratePolls();
        ResetTimer();

       
    }
   
    [Server]
    void ResetTimer()
    {
        voteTimer = voteTimerMax;
    }
  
    #endregion
 
}
