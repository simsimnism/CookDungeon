using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    //public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDictionary();
    }


    //이 메서드는 JSON 파일을 로드하고, 이를 Loader 타입으로 변환
    public Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    //JSON 파일을 로드하고 그 데이터를 딕셔너리로 변환하여 반환
    public Dictionary<Key, Value> LoadData<Key, Value, Loader>(string path) where Loader : ILoader<Key, Value>
    {
        Loader loader = LoadJson<Loader, Key, Value>(path);
        return loader.MakeDictionary();
    }

    public void Init()
    {

    }

}
