using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    private void Awake()
    {
        //탱크를 생성하는 함수 호출
        CreateTank();
        //포톤 클라우드의 네트워크 메시지 수신을 다시 연결
        PhotonNetwork.isMessageQueueRunning = true;
    }

    //탱크를 생성하는 함수
    void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }
}
