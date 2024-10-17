using UnityEngine;

public class DataManager 
{
    private MonsterDataLoader _monsterDataLoader = new MonsterDataLoader();

    public void Init()
    {
        // 몬스터 데이터 로드
        _monsterDataLoader.LoadMonsterData("json/Monsters");
    }

    public MonsterDataSO GetMonsterDataByName(string name)
    {
        return _monsterDataLoader.GetMonsterDataByName(name);
    }

    public MonsterDataSO GetMonsterDataById(int id)
    {
        return _monsterDataLoader.GetMonsterDataById(id);
    }
}
