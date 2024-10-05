using UnityEngine;

public class WeaponLoader : MonoBehaviour
{
    private WeaponDataList weaponDataList;

    // JSON 파일을 로드하고 파싱하는 함수
    public void LoadWeaponData()
    {
        // Resources 폴더에 있는 "weapons.json" 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>("weapons");

        // JSON 파일이 존재하는지 확인
        if (jsonFile != null)
        {
            // JSON 파일을 WeaponDataList 타입으로 변환
            weaponDataList = JsonUtility.FromJson<WeaponDataList>(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다.");
        }
    }

    // 무기 데이터를 가져오는 함수
    public WeaponData GetWeaponDataByName(string weaponName)
    {
        foreach (WeaponData weaponData in weaponDataList.weapons)
        {
            if (weaponData.name == weaponName)
            {
                return weaponData; // 해당 무기 데이터를 반환
            }
        }

        Debug.LogError("해당 이름의 무기를 찾을 수 없습니다.");
        return null;
    }
}
