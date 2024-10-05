using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // 풀에 저장할 각 몬스터 타입의 프리팹과 풀 사이즈
    [System.Serializable]
    public class Pool
    {
        public string tag;       // 풀의 태그 (예: 몬스터 ID 또는 이름)
        public GameObject prefab;  // 풀에서 관리할 프리팹
        public int size;         // 풀에서 미리 생성할 객체의 수
    }

    public List<Pool> pools;  // 여러 풀을 관리하기 위한 리스트
    public Dictionary<string, Queue<GameObject>> poolDictionary;  // 풀의 태그로 접근 가능한 딕셔너리

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 각 풀에 대해 객체를 미리 생성하고 큐에 넣는다.
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // 비활성화하여 대기 상태로 둔다.
                objectPool.Enqueue(obj); // 큐에 추가
            }

            poolDictionary.Add(pool.tag, objectPool); // 딕셔너리에 추가
        }
    }

    // 객체를 풀에서 가져오는 함수
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("풀에 없는 태그: " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // 풀에서 가져온 객체를 활성화하고 위치와 회전을 설정
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // 다시 풀에 반환하기 위해 큐에 객체를 재추가
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
