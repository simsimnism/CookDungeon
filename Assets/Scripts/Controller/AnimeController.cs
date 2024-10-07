using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimeController : MonoBehaviour
{

    private float speed;
    private PlayerManager pm;
    private SpriteRenderer sr;
    Rigidbody2D rbody;


    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        speed = Managers.Player.speed;


    }
    void Start()
    {

    }

    void Update()
    {

    }

    void StarterImage()
    {
    }

    void PlayAnime()
    {

    }

}
