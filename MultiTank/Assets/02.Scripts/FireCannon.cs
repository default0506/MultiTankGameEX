using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour {
    //cannon 프리팹을 연결할 변수
    private GameObject cannon = null;
    //포탄 발사 사운드 파일
    private AudioClip fireSfx = null;
    //AudioSource 컴포넌트를 할당할 변수
    private AudioSource sfx = null;
    //cannon 발사 지점
    public Transform firePos;
    //PhotonView 컴포넌트를 할당할 변수
    private PhotonView pv = null;

    private void Awake()
    {
        //cannon 프리팹을 Resources폴더에서 불러와 변수에 할당
        cannon = (GameObject)Resources.Load("Cannon");
        //포탄 발사 사운드 파일을 Resources폴더에서 불러와 변수에 할당.
        fireSfx = Resources.Load<AudioClip>("CannonFire");
        //AudioSource컴포넌트를 할당
        sfx = GetComponent<AudioSource>();
        //PhotonView컴포넌트를 pv변수에 할당
        pv = GetComponent<PhotonView>();
    }
	// Update is called once per frame
	void Update () {
        //PhotonView가 자신의 것이고 마우스 왼쪽 버튼 클릭 시 발사 로직을 수행
        if(pv.isMine && Input.GetMouseButtonDown(0))
        {
            //자신의 탱크일 경우는 로컬함수를 호출에 포탄을 발사
            Fire();
            //원격 네트워크 플레이어의 탱크에 RPC로 원격으로 Fire함수를 호출
            pv.RPC("Fire", PhotonTargets.Others, null);
        }
	}

    [PunRPC]
    void Fire()
    {
        //발사 사운드 발생
        sfx.PlayOneShot(fireSfx, 1.0f);
        Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
