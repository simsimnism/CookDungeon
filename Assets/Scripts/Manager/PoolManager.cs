using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//풀링 매니저
public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            // new 뒤에는(),[]등이 
            pools[index] = new List<GameObject>();
        }
        Debug.Log(pools.Length);
        
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 있는 게임 오브젝트 접근
            // 발견하면 select 변수에 할당
        foreach(GameObject item in pools[index]) 
        { 
            if (item.activeSelf) 
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 못 찾았으면?
        if (!select)
        {
            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
        }
        return select;
    }

}
