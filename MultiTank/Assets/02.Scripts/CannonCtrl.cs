﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour {
    private Transform tr;
    public float rotSpeed = 500.0f;
    //PhotonView컴포넌트 변수
    private PhotonView pv = null;
    //원격 네트워크 탱크의 포신 회전 각도를 저장할 변수
    private Quaternion currRot = Quaternion.identity;

	// Use this for initialization
	void Awake () {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        //PhotonView의 Observed속성을 이 스크립트로 설정
        pv.ObservedComponents[0] = this;
        //PhotonView의 동기화 속성을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        //초기 회전값 설정
        currRot = tr.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        //자신의 탱크일 때만 조정
        if(pv.isMine)
        {
            float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
            tr.Rotate(angle, 0, 0);
        }
        else
        {
            //현재 회전 각도에서 수신받은 실시간 회전 각도로 부드럽게 회전
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 3.0f);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
