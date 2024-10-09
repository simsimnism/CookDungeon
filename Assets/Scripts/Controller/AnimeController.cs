using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimeController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float speed = 1;
    private Animator anime;
    private PlayerManager pm;
    private SpriteRenderer rend;
    Rigidbody2D rbody;
    private PlayerAttack pa;
    private PlayerController pc;

    


    void Awake()
    {
        pm = GetComponent<PlayerManager>();
        anime = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        pa = GetComponent<PlayerAttack>();
        pc = GetComponent<PlayerController>();
        speed = Managers.Player.moveSpeed;


    }
    void Start()
    {

    }

    void Update()
    {

    }

    //플레이어의 정지상태 애니매이션
    void PlayerStand()
    {
        if(!Managers.GM.IsMoving) 
        {
            anime.SetTrigger("Stand");
        }

    }

    //플레이어의 이동상태 애니매이션
    void PlayerSideWalk()
    {
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            anime.SetTrigger("RightWalk");
        }

    }
    void PlayerUpDownWalk()
    {
        if(Input.GetKeyDown(KeyCode.W)) 
        {
            anime.SetTrigger("UpWalk");
        
        }
    }
    
    void PlayerDash() 
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            anime.SetTrigger("Dash");
        }
    }

    //플레이어의 공격 상태 애니매이션
    void Attackainime()
    {
        if(Input.GetMouseButton(0)) 
        {
            anime.SetTrigger("Attack1");
        }

    }



}
