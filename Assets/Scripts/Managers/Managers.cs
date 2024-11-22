using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();
    GameManager _game = new GameManager();
    PlayerManager _player = new PlayerManager();
    InventoryManager _inventory = new InventoryManager();
    UIPopupManager _popup = new UIPopupManager();
    SkillManager _skill = new SkillManager();   

    SoundManager _sound;
  

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }

    //게임의 상태를 관리하는 매니저
    public static GameManager GM { get { Init(); return Instance._game; } }

    //플레이어의 수치 등을 관리하는 매니저
    public static PlayerManager Player { get { Init(); return Instance._player; } }

    //인벤토리 자체의 상태를 관리하는 매니저
    public static InventoryManager Inventory { get { Init(); return Instance._inventory; } }

    //UI의 기능중에서도 팝업만을 관리하는 매니저
    public static UIPopupManager Popup { get { Init(); return Instance._popup; } }  

    //스킬의 생성 삭제 등 스킬 전반을 관리하는 매니저
    public static SkillManager Skill { get { Init(); return Instance._skill; } }


    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();  // 매 프레임 입력 업데이트
    }

    void FixedUpdate()
    {
        _game.HandleGameState();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            GameObject sound = GameObject.Find("@Sound");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                sound = new GameObject { name = "@Sound" };
                sound.transform.SetParent(go.transform);
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetOrAddComponent<Managers>();

            s_instance._sound = sound.GetOrAddComponent<SoundManager>();

            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._player.Init();
        }
    }


    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
