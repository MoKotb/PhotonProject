using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] string playerPrefab;
    [SerializeField] Transform LeftWall;
    [SerializeField] Transform RightWall;
    [SerializeField] Transform TopWall;
    [SerializeField] Transform BottomWall;

    void Start()
    {
        float xPosition = Random.Range(LeftWall.position.x - 1, RightWall.position.x - 1);
        float zPosition = Random.Range(TopWall.position.z - 1, BottomWall.position.z - 1);
        Vector3 vector3 = new Vector3(xPosition,0,zPosition);
        PhotonNetwork.Instantiate(playerPrefab, vector3, Quaternion.identity);
    }
    
}
