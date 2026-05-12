using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Framework;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class SpawnMonsterHandler : MonoBehaviour
{
    public DrawCardPanel drawCardPanel;
    public List<Transform> spawnPoints;
    public TextAsset monsterConfig;
    public TextAsset difficultyConfig;

    private readonly List<List<int>> levelData = new();
    private readonly List<List<List<List<int>>>> data = new();

    private readonly Dictionary<int, EnemyBase> prefabs = new();

    private IEnumerator handler;
    private Action onRowEnd;
    private Action onWaveEnd;
    private Action onLevelEnd;

    private int currentLevel = 1;
    private bool waitingForCard;
    private bool levelSpawningDone;
    private int currentWaveIndex;
    private int totalWavesInLevel;

    // Start is called before the first frame update
    void Start()
    {
        LoadMonster();
        LoadLevelData();
        LoadMonsterPrefabs();

        StartLevel(currentLevel);
    }

    void Update()
    {
        if (levelSpawningDone && !waitingForCard)
        {
            var em = ModulesManager.Get<EnemyManager>();
            if (em.enemies.Count == 0)
            {
                levelSpawningDone = false;
                onLevelEnd?.Invoke();
            }
        }

        if (waitingForCard && !drawCardPanel.gameObject.activeSelf)
        {
            waitingForCard = false;
            Time.timeScale = 1;
            currentLevel++;
            if (currentLevel < levelData.Count)
                StartLevel(currentLevel);
        }
    }

    public void StartLevel(int level)
    {
        levelSpawningDone = false;
        StartSpawn(level, null, NextWave, () =>
        {
            drawCardPanel.OpenDrawCardPanel();
            Time.timeScale = 0;
            waitingForCard = true;
        });
    }

    public void StartSpawn(int level, Action rowEnd, Action waveEnd, Action levelEnd)
    {
        onRowEnd = rowEnd;
        onWaveEnd = waveEnd;
        onLevelEnd = levelEnd;
        handler = SpawnLevel(level);
        handler.MoveNext();
    }

    public void NextWave()
    {
        handler.MoveNext();
    }


    private IEnumerator SpawnLevel(int level)
    {
        var ld = levelData[level];
        totalWavesInLevel = ld.Count;
        currentWaveIndex = 0;
        Debug.Log("start" );
        RefreshLevelUI();

        foreach (var waveHardness in ld)
        {
            currentWaveIndex++;
            RefreshLevelUI();
            _ = SpawnWave(waveHardness, Random.Range(4, 6));
            yield return null;
        }

        levelSpawningDone = true;
    }

    async UniTask SpawnWave(int hardness, int total)
    {
        var r = Rand(hardness, total);
        
        foreach (var row in r)
        {
            SpawnOneRow(row[0], spawnPoints[0].position);
            SpawnOneRow(row[1], spawnPoints[1].position);
            SpawnOneRow(row[2], spawnPoints[2].position);
            SpawnOneRow(row[3], spawnPoints[3].position);
            SpawnOneRow(row[4], spawnPoints[4].position);
            SpawnOneRow(row[5], spawnPoints[5].position);

            onRowEnd?.Invoke();
            await UniTask.Delay(1000);
        }
        
        await UniTask.Delay(3000);
        onWaveEnd?.Invoke();
    }

    private void SpawnOneRow(int id, Vector3 position)
    {
        if (id == 0) return;
        var prefab = prefabs[id];
        Instantiate(prefab, position, Quaternion.Euler(0, 180, 0));
    }

    private List<List<int>> Rand(int hardness, int total)
    {
        var result = new List<List<int>>();
        while (total > 0)
        {
            var filter = data[hardness].Where(formation => formation.Count <= total).ToList();
            var formation = filter[Random.Range(0, filter.Count)];
            result.AddRange(formation);
            total -= formation.Count;
        }
        return result;
    }

    void RefreshLevelUI()
    {
        LevelWaveUI.instance.RefreshUI(
            currentLevel,
            currentWaveIndex,
            totalWavesInLevel
        );
    }

    // Load

    void LoadMonsterPrefabs()
    {
        prefabs.Add(1, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/XiaoHuoGuai1"));
        prefabs.Add(2, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/XiaoHuoGuai2"));
        prefabs.Add(3, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/XiaoHuoGuai3"));
        prefabs.Add(4, Resources.Load<Phoenix>("Prefab/Enemy/Phoenix"));
        prefabs.Add(5, Resources.Load<Monkey>("Prefab/Enemy/Monkey"));
        prefabs.Add(6, Resources.Load<JingCu>("Prefab/Enemy/JingCu"));
        prefabs.Add(7, Resources.Load<Dog>("Prefab/Enemy/Dog"));
        prefabs.Add(8, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/HuoShu"));
    }

    void LoadMonster()
    {
        // var text = Resources.Load<TextAsset>("BHYX/Monsters").text;
        var text = monsterConfig.text;
        var lines = text.Split("\n");
        foreach (var line in lines)
        {
            var num = line.Split(",");
            var hardness = int.Parse(num[0]);

            while (data.Count <= hardness)
            {
                data.Add(new List<List<List<int>>>());
            }

            var row = int.Parse(num[1]);
            if (row == 0)
            {
                data[hardness].Add(new List<List<int>>());
            }

            var formation = data[hardness].Count - 1;

            data[hardness][formation].Add(new List<int>());
            data[hardness][formation][row].Add(int.Parse(num[2]));
            data[hardness][formation][row].Add(int.Parse(num[3]));
            data[hardness][formation][row].Add(int.Parse(num[4]));
            data[hardness][formation][row].Add(int.Parse(num[5]));
            data[hardness][formation][row].Add(int.Parse(num[6]));
            data[hardness][formation][row].Add(int.Parse(num[7]));
        }
    }

    void LoadLevelData()
    {
        var text = difficultyConfig.text;
        var lines = text.Split("\n");
        foreach (var line in lines)
        {
            var waves = new List<int>();
            var num = line.Split(",");
            for (var i = 0; i < num.Length; i++)
            {
                if (int.TryParse(num[i], out var hardness))
                {
                    waves.Add(hardness);
                }
            }

            levelData.Add(waves);
        }
    }
}