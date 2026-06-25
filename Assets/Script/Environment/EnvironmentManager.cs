using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    [SerializeField] private Vector3 spawnPosition = new Vector3(-13.07135f, 5.621923f, 11.15146f);

    private GameObject _currentEnv;
    private int _currentEnvIndex = -1;

    private readonly string[] envPrefabPaths = new string[]
    {
        "Prefab/Enviroment/Spring",
        "Prefab/Enviroment/Summer",
        "Prefab/Enviroment/Autumn",
        "Prefab/Enviroment/Winter"
    };

    private readonly string[] bgmNames = new string[]
    {
        "LevelSpri",
        "LevelSum",
        "LevelAut",
        "LevelWin"
    };

    private void Awake()
    {
        Instance = this;
    }


    public void Init()
    {
        LoadEnvironment(GetEnvIndex(GameState.currentLevel));
    }


    public void CheckAndSwitch(int currentLevel)
    {
        int envIndex = GetEnvIndex(currentLevel);
        if (envIndex == _currentEnvIndex)
            return;

        LoadEnvironment(envIndex);
    }

    private static int GetEnvIndex(int currentLevel)
    {
        return ((currentLevel - 1) / 5) % 4;
    }

    private void LoadEnvironment(int index)
    {
        Destroy(_currentEnv);
        _currentEnv = null;

        _currentEnvIndex = index;
        string path = envPrefabPaths[index];

        GameObject prefab = Resources.Load<GameObject>(path);
        _currentEnv = Instantiate(prefab);
        _currentEnv.transform.position = spawnPosition;

        // 切换到对应场景 BGM
        SoundManager.PlayBGM("Audio/BGM/" + bgmNames[index]);
    }
}