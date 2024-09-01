using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;

// 플레이어의 이동과 애니매이션 적용
// 플레이어의 데미지 처리 적용
// 플레이어의 대쉬 적용

public class PlayerController : MonoBehaviour
{
    //이동속도
    public float speed = 3.0f;
    //애니매이션 이름
    public string upAinme = "PlyerUp";
    public string downAinme = "PlyerDown";
    public string rightAinme = "PlyerRight";
    public string LeftAinme = "PlyerLeft";
    public string deadAinme = "PlayerDead";
    string nowAnimation = "";
    string oldAnimation = "";

    float axisH;
    float axisV;
    public float angleZ = -90.0f;

    Rigidbody2D rbody;
    bool isMoving = false;

    //대미지 처리
    public static int hp = 3;
    public static string gameState;
    bool inDamage = false;
    private string deadAnime;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        oldAnimation = downAinme;
        gameState = "playing";
        
    }

    void Update()
    {
        //게임 중이 아니거나 대미지를 받는 중에는 아무것도 하지 않음
        if(gameState != "playing" || inDamage)
        {
            return;
        }

        if (!isMoving == false) 
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
        }
        //키 입력으로 이동각도 구하기
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH,fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);
        if (angleZ >= -45 && angleZ < 45) 
        {
            //오른쪽
            nowAnimation = rightAinme;

        }
        else if(angleZ >= 45 && angleZ <= 135) 
        {
            //위쪽
            nowAnimation = upAinme;

        }
        else if (angleZ >=-135 && angleZ <=-45 )
        {
            //아래쪽
            nowAnimation = downAinme;
        }
        else 
        {
            //왼쪽
            nowAnimation = LeftAinme;
        }

        //애니매이션 변경하기
        if (nowAnimation != oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation);
        }
        
    }
    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return ;
        }
        if (inDamage) 
        {
            float val = Mathf.Sin(Time.time*50);
            Debug.Log(val);
            if (val > 0) 
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else 
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return ;

        }
        
        //이동속도 변경하기
        rbody.velocity = new Vector2(axisH, axisV) * speed;


        
    }
    public void SetAxis(float h , float v)
    {
        axisH = h;
        axisV = v; 
        if (axisH == 0 && axisV == 0)
        {
            isMoving = false;

        }
        else 
        {
            isMoving = true;
        }
    }
    // p1에서 p2까지의 각도를 계산
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV !=0)
        {
            // 이동중이면 각도를 변경
            // p1과 p2의 차이 구하기
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            // 아크 탄젠트 함수로 각도 구하기
            float rad = Mathf.Atan2(dx, dy);
            angle = rad * Mathf.Rad2Deg;
        }
        else 
        {
            //정지중이면 이전 각도 유지
            angle = angleZ;
        }
        return angle;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    void GetDamage(GameObject enemy)
    {
        if (gameState == "playing")
        {
            hp--;
            if(hp > 0)
            {
                rbody.velocity = new Vector2(0, 0);
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x*4,toPos.y*4), ForceMode2D.Impulse);
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else 
            {
                GameOver();
            }
        }
    }

    void DamageEnd()
    {
        inDamage = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    void GameOver() 
    {
        Debug.Log("");
        gameState = "gameover";
        GetComponent<CircleCollider2D>().enabled = false;
        rbody.velocity = new Vector2(0, 0);
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        GetComponent<Animator>().Play(deadAnime);
        Destroy(gameObject, 1.0f);
    }
}