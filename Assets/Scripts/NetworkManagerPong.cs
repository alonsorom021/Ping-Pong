using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{

    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;
    public GameObject ballPrefab;
    public Ball ball;
    

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        // spawnear ball si hay 2 players
        if (numPlayers == 2)
        {
            var tempBall = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(tempBall);
            ball = tempBall.GetComponent<Ball>();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // destroy ball
        if (ball != null)
        {
            ball.textScore.text = "0 - 0";
            NetworkServer.Destroy(ball.gameObject);
        }
            

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }
}