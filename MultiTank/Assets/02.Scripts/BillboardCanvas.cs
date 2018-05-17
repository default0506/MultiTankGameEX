using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour {

    private Transform tr;
    private Transform mainCameraTr;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        //스테이지에 있는 메인 카메라의 Transform 컴포넌트를 추출
        mainCameraTr = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        tr.LookAt(mainCameraTr);
	}
}
