using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static EnemyData;

// 적 데이터를 JSON 파일로부터 로드하는 클래스
public class EnemyDataLoader : MonoBehaviour
{
    // JSON 파일 이름을 설정
    public string fileName = "MonsterData.json";

    // JSON 파일을 읽어서 EnemyData 객체로 변환하는 메서드
    public List<EnemyData> LoadEnemyData()
    {
        // JSON 파일의 경로를 설정
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // 파일 내용을 읽어서 문자열로 저장
            string json = File.ReadAllText(filePath);

            // JSON 문자열을 EnemyDataList 객체로 변환하고, enemyData 리스트를 반환
            return JsonUtility.FromJson<EnemyDataList>(json).monsters;
        }
        else
        {
            // 파일을 찾지 못한 경우 에러 로그를 출력
            Debug.LogError("Cannot find file!");
            return new List<EnemyData>();
        }
    }
}
