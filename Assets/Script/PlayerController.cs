using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool move_left;
    bool move_right;
    Vector2 dir;
    float max_speed;

    // Start is called before the first frame update
    void Start()
    {
        move_left = false;
        move_right = false;
        dir.x = 0f;
        dir.y = 0f;
        max_speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();

        if (move_left && move_right) dir.x = 0f;
        else if (move_right) dir.x = 1f;
        else if (move_left) dir.x = -1f;
        else dir.x = 0;

        Flip(dir.x);

        dir.Normalize();

        transform.Translate(dir * max_speed * Time.deltaTime);
    }

    void Flip(float x_dir)
    {
        Vector3 object_scale;

        if (x_dir < 0)
        {
            object_scale = transform.localScale; //objectScale에 변화를 줄 로컬 스케일의 값을 가져온다
            object_scale.x = -Mathf.Abs(object_scale.x); // x의 값을 -1로 바꾸면서 회전한다.
            transform.localScale = object_scale; //로컬스케일의 값을 objectScale로 하면서 로컬스케일이 변화한다
        }
        else if (x_dir > 0)
        {
            object_scale = transform.localScale;
            object_scale.x = Mathf.Abs(object_scale.x); // x의 값을 1로 바꾸면서 원래 모습으로 한다.
            transform.localScale = object_scale;
        }
    }

    void ProcessInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!move_left)
                move_left = true;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!move_right)
                move_right = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            move_left = false;
        }

        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            move_right = false;
        }
    }
}
