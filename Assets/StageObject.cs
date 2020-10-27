using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageObject : MonoBehaviour
{
    private GameObject target;

    public int stage;
    public int type;
    public int num;
    public GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && target == this.gameObject && stage == gamemanager.GetComponent<GameManager>().stage)
        {
            switch (type)
            {
                case 0:
                    Debug.Log("전투 스테이지");

                    break;
                case 1:
                    Debug.Log("회복 스테이지");

                    break;
                case 2:
                    Debug.Log("상점 스테이지");

                    break;
                case 3:
                    Debug.Log("보스 스테이지");

                    break;
                case 4:
                    Debug.Log("빈 스테이지");

                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
            if (target == this.gameObject)
            {  //타겟 오브젝트가 스크립트가 붙은 오브젝트라면

                // 여기에 실행할 코드를 적습니다.
                Debug.Log(this.name);
            }
        }
    }



    void CastRay() // 유닛 히트처리 부분.  레이를 쏴서 처리합니다. 
    {
        target = null;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        { //히트되었다면 여기서 실행

            //Debug.Log (hit.collider.name);  //이 부분을 활성화 하면, 선택된 오브젝트의 이름이 찍혀 나옵니다. 

            target = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정
            gamemanager.GetComponent<GameManager>().selected_stage = target;
        }

    }
}
