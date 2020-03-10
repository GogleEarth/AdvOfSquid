using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MOVE_FORCE = 1f;
    public float COF; // 마찰계수
    public float MAX_SPEED = 10f;
    public float JUMP_FORCE = 10f;
    public float DASH_COOLTIME = 0.5f;
    public float DASH_FORCE = 5f;
    public int MAX_AIR_DASH_CNT = 1;
    public int MAX_AIR_JUMP_CNT = 0;
    
    bool move_left;
    bool move_right;
    bool onground;
    bool down_key;
    bool jump_key;
    Vector2 dir;
    float curr_speed;
    float force;
    int air_dash_count;
    int air_jump_count;
    Rigidbody2D squid_rigidbody;
    float jump_force;
    float dash_cooltime;

    void Start()
    {
        move_left = false;
        move_right = false;
        onground = false;
        down_key = false;
        jump_key = false;
        dir.x = 0f;
        dir.y = 0f;
        force = 0f;
        curr_speed = 0f;
        air_dash_count = 0;
        air_jump_count = 0;
        jump_force = 0f;
        dash_cooltime = DASH_COOLTIME;
        squid_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (dash_cooltime > 0f) dash_cooltime -= Time.deltaTime;

        ProcessInput();

        if (move_left && move_right) dir.x = 0f;
        else if (move_right) dir.x = 1f;
        else if (move_left) dir.x = -1f;
        else if (curr_speed <= 0f) dir.x = 0;

        Flip(dir.x);

        dir.Normalize();

        if (Input.GetAxis("Horizontal") != 0)
            force += MOVE_FORCE;
        else
            force = 0f;

        if (jump_key)
        {
            if (jump_force < JUMP_FORCE) jump_force += 1f;
            else jump_force = JUMP_FORCE;
        }

        float prev_speed = curr_speed;
        if (onground) curr_speed += (force - (2 * COF * 9.8f)) * Time.deltaTime; 
        else curr_speed += force * Time.deltaTime;

        if (curr_speed >= MAX_SPEED) curr_speed = MAX_SPEED;
        else if (curr_speed <= 0) curr_speed = 0;

        transform.Translate(dir * curr_speed * Time.deltaTime);
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!move_left)
                move_left = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!move_right)
                move_right = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!down_key)
                down_key = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!jump_key && onground)
                jump_key = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            move_left = false;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            move_right = false;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            down_key = false;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            Jump();
            jump_key = false;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Dash();
        }
    }
    
    private void Jump()
    {
        if (onground)
        {
            squid_rigidbody.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
            jump_force = 0f;
        }
        else if (down_key)
        {
            squid_rigidbody.AddForce(Vector2.up * 3 * -JUMP_FORCE, ForceMode2D.Impulse);
        }
        else
        {
            if (air_jump_count < MAX_AIR_JUMP_CNT)
            {
                squid_rigidbody.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
                air_jump_count++;
            }
        }
    }

    private void Dash()
    {
        if (!onground)
        {
            if (move_left)
            {
                if (air_dash_count < MAX_AIR_DASH_CNT)
                {
                    squid_rigidbody.AddForce(Vector2.right * -DASH_FORCE, ForceMode2D.Impulse);
                    air_dash_count++;
                }
            }
            else if (move_right)
            {
                if (air_dash_count < MAX_AIR_DASH_CNT)
                {
                    squid_rigidbody.AddForce(Vector2.right * DASH_FORCE, ForceMode2D.Impulse);
                    air_dash_count++;
                }
            }
        }
        else
        {
            if (dash_cooltime <= 0f)
            {
                if (move_left)
                    squid_rigidbody.AddForce(Vector2.right * -DASH_FORCE, ForceMode2D.Impulse);
                else if (move_right)
                    squid_rigidbody.AddForce(Vector2.right * DASH_FORCE, ForceMode2D.Impulse);
                dash_cooltime = DASH_COOLTIME;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            onground = true;
            air_dash_count = 0;
            air_jump_count = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground") onground = false;
    }
}
