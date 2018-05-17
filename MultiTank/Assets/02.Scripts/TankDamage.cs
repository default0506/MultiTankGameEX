using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankDamage : MonoBehaviour {
    //탱크 폭파 후 투명 처리를 위한 MeshRenderer컴포넌트 배열
    private MeshRenderer[] renderers;

    //탱크 폭발 효과 프리팹을 연결할 변수
    private GameObject expEffect = null;

    //탱크의 초기 생명치
    private int initHp = 100;
    //탱크의 현재 생명치
    private int currHp = 0;

    //탱크 하위의 Canvas 객체를 연결할 변수
    public Canvas hudCanvas;
    //Filled타입의 Image UI 항목을 연결할 변수
    public Image hpBar;

    private void Awake()
    {
        //탱크 모델의 모든 MeshRenderer컴포넌트를 추출한 후 배열에 할당
        renderers = GetComponentsInChildren<MeshRenderer>();

        //현재 생명치를 초기 생명치로 초깃값 설정
        currHp = initHp;

        //탱크 폭발 시 생성시킬 폭발 효과를 로드
        expEffect = Resources.Load<GameObject>("Large Explosion");

        //Filled이미지 색상을 녹색으로 설정
        hpBar.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        //충돌한 Collider의 태그 비교
        if(currHp > 0 && other.tag == "CANNON")
        {
            currHp -= 20;

            //현재 생명치 백분율 = (현재 생명치) / (초기 생명치)
            hpBar.fillAmount = (float)currHp / (float)initHp;

            //생명 수치에 따라 Filed 이미지의 색상을 변경
            if (hpBar.fillAmount <= 0.4f)
                hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.6f)
                hpBar.color = Color.yellow;

            if(currHp <= 0)
            {
                StartCoroutine(this.ExplosionTank());
            }
        }
    }

    //폭발 효과 생성 및 리스폰 코루틴 함수
    IEnumerator ExplosionTank()
    {
        //폭발 효과 생성
        Object effect = GameObject.Instantiate(expEffect,
            transform.position, Quaternion.identity);
        Destroy(effect, 3.0f);

        //HUD 비활성화
        hudCanvas.enabled = false;

        //탱크 투명 처리
        SetTankVisible(false);
        //3초 동안 기다렸다가 활성화하는 로직을 수행
        yield return new WaitForSeconds(3.0f);

        //Filled이미지 초깃값으로 환원
        hpBar.fillAmount = 1.0f;
        //Filled이미지 색상을 녹색으로 설정
        hpBar.color = Color.green;
        //HUD 활성화
        hudCanvas.enabled = true;

        //리스폰 시 생명 초깃값 설정
        currHp = initHp;
        //탱크를 다시 보이게 처리
        SetTankVisible(true);
    }

    //MeshRenderer를 활성/비활성화하는 함수
    void SetTankVisible(bool isVisible)
    {
        foreach(MeshRenderer _renderer in renderers)
        {
            _renderer.enabled = isVisible;
        }
    }
}
