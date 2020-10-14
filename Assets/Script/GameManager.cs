using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool game_start;
    // Start is called before the first frame update
    public int stage = 0;
    public int floor = 0;
    public GameObject selected_stage;

    void Start()
    {
        game_start = false;
        selected_stage = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(selected_stage)
        {
            switch (selected_stage.GetComponent<StageObject>().type)
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
            selected_stage = null;
        }
    }

    public void SetGameStart(bool start)
    {
        game_start = start;
    }
}
