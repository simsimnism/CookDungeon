using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{

    public float 이동속도 = 5f; // 일반 이동 속도
    public float 대쉬속도 = 10f; // 대쉬 속도

    // Start is called before the first frame update
    void Start()
    {
        float 가로입력 = Input.GetAxis("Horizontal");
        float 세로입력 = Input.GetAxis("Vertical");

        // 상하좌우 이동
        Vector3 이동 = new Vector3(가로입력, 세로입력, 0f) * 이동속도 * Time.deltaTime;
        transform.Translate(이동, Space.World);

        // 좌우 이동할 때 캐릭터가 해당 방향을 바라보도록 회전
        if (가로입력 != 0f)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(가로입력, 0f, 0f));
        }

        // Shift 키를 누르고 있는 동안 대쉬
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 대쉬이동 = new Vector3(가로입력, 세로입력, 0f) * 대쉬속도 * Time.deltaTime;
            transform.Translate(대쉬이동, Space.World);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        
    }
}
