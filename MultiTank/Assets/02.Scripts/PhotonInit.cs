using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviour {
    //App의 버전 정보
    public string version = "v1.0";

    //플레이어 이름을 입력하는 UI 항목 연결
    public InputField userId;

    private void Awake()
    {
        //포톤 클라우드에 접속
        PhotonNetwork.ConnectUsingSettings(version);
    }

    //포톤 클라우드에 정상적으로 접속한 후 로비에 입장하면 호출되는 콜백 함수
    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby!");
        userId.text = GetUserId();
        //PhotonNetwork.JoinRandomRoom();
    }

    //로컬에 저장된 플레이어 이름을 반환하거나 생성하는 함수
    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");

        if (string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        }

        return userId;
    }

    //Join random Room버튼 클릭 시 호출되는 함수
    public void OnClickJoinRandomRoom()
    {
        //로컬 플레이어의 이름을 설정
        PhotonNetwork.player.name = userId.text;
        //플레이어 이름을 저장
        PlayerPrefs.SetString("USER_ID", userId.text);

        //무작위로 추출된 룸으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    //무작위 룸 접속에 실패한 경우 호출되는 콜백 함수
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms!");
        //룸 생성
        PhotonNetwork.CreateRoom("MyRoom");
    }

    //룸에 입장하면 호출되는 콜백 함수
    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");
        
        StartCoroutine(this.LoadBattleField());
    }

    //룸 씬으로 이동하는 코루틴 함수
    IEnumerator LoadBattleField()
    {
        //씬을 이동하는 동한 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        PhotonNetwork.isMessageQueueRunning = false;
        //백그라운드로 씬 로딩
        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");
        yield return ao;
    }

    private void OnGUI()
    {
        //화면 좌측 상단에 접속 과정에 대한 로그를 출력
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
