using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerVote : NetworkManager
{
    public static NetworkManagerVote instance;
    public GameObject VoteManager;

    public override void Awake()
    {
        instance = this;
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameObject voteObj = Instantiate(VoteManager);
        NetworkServer.Spawn(voteObj);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
     
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
      
        base.OnClientSceneChanged(conn);
    }



}
