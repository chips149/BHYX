using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnMonsterHandler : MonoBehaviour
{
    public DrawCardPanel drawCardPanel;
    public List<Transform> spawnPoints;
    public TextAsset monsterConfig;
    public TextAsset difficultyConfig;

    [Header("怪物属性提升")] 
    public int roundInterval = 10;
    public float hpGrowthPerTier = 0.2f;
    public float speedGrowthPerTier = 0.1f;

    private readonly List<List<int>> levelData = new();
    private readonly List<List<List<List<int>>>> data = new();

    private readonly Dictionary<int, EnemyBase> prefabs = new();
    public static SpawnMonsterHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        LoadMonster();
        LoadLevelData();
        LoadMonsterPrefabs();

    }

       public void StartSpawn()
    {
        if (GameState.currentLevel >= levelData.Count)
        {
            if (GameState.isEndlessMode)
            {
                GameState.spawnOver = false;
                _ = SpawnEndlessLevel();
            }
            else
            {
                GameUIManager.instance.Win();
            }
            return;
        }

        GameState.spawnOver = false;
        _ = SpawnLevel(GameState.currentLevel);
    }

       

    private async UniTask SpawnLevel(int level)
    {
        var ld = levelData[level];
        
        var total = ld.Count;
        var count = 0;
        LevelWaveUI.instance?.RefreshUI(level, 0, total);
        foreach (var waveHardness in ld)
        {
            if (GameState.isGameOver) return;
            
            count++;
            GameState.onWaveSpawnOver?.Invoke(count, total);
            await SpawnWave(waveHardness, Random.Range(4, 6));
        }
        
        GameState.onSpawnComplete?.Invoke();
    }

    private async UniTask SpawnEndlessLevel()
    {
        var round = GameState.currentLevel - levelData.Count + 1;

        var waveCount = Mathf.Clamp(3 + round / 3, 3, 10);
        var rowsPerWave = Mathf.Clamp(4 + round / 2, 4, 12);

        var hardnessPool = new int[] { 0, 1, 2, 4 };
        var startIndex = (round - 1) % 4;

        LevelWaveUI.instance?.RefreshUI(GameState.currentLevel, 0, waveCount);
        for (int i = 0; i < waveCount; i++)
        {
            if (GameState.isGameOver) return;

            var hardness = hardnessPool[(startIndex + i) % 4];
            GameState.onWaveSpawnOver?.Invoke(i + 1, waveCount);
            await SpawnWave(hardness, rowsPerWave);
        }

        GameState.onSpawnComplete?.Invoke();
    }

    async UniTask SpawnWave(int hardness, int total)
    {
        var r = Rand(hardness, total);

        var rowCount = 0;
        foreach (var row in r)
        {
            if (GameState.isGameOver) return;
            if (this == null) return;
            
            SpawnOneRow(row[0], spawnPoints[0].position);
            SpawnOneRow(row[1], spawnPoints[1].position);
            SpawnOneRow(row[2], spawnPoints[2].position);
            SpawnOneRow(row[3], spawnPoints[3].position);
            SpawnOneRow(row[4], spawnPoints[4].position);
            SpawnOneRow(row[5], spawnPoints[5].position);

            rowCount++;
            GameState.onRowSpawnOver?.Invoke(rowCount, total);
            await UniTask.Delay(1000);
        }
    }

    private void SpawnOneRow(int id, Vector3 position)
    {
        if (id == 0) return; 
        //var prefab = prefabs[id];
        
        if (!prefabs.TryGetValue(id, out var prefab) || prefab == null)
        {
            return;
        }

        var enemy = Instantiate(prefab, position, Quaternion.Euler(0, 180, 0));

        var tier = (GameState.currentLevel - 1) / roundInterval;
        if (tier > 0)
        {
            enemy.hp = Mathf.RoundToInt(enemy.hp * (1.0f + tier * hpGrowthPerTier));
            enemy.maxHp = enemy.hp;
            enemy.speed *= (1.0f + tier * speedGrowthPerTier);
            enemy.UpdateHealthDisplay();
        }
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

    void LoadMonsterPrefabs()
    {
        prefabs.Add(1, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/XiaoHuoGuai1"));
        prefabs.Add(2, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/XiaoHuoGuai2"));
        prefabs.Add(3, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/XiaoHuoGuai3"));
        prefabs.Add(4, Resources.Load<Phoenix>("Prefab/Enemy/Phoenix"));
        prefabs.Add(5, Resources.Load<Monkey>("Prefab/Enemy/Dog"));
        prefabs.Add(6, Resources.Load<JingCu>("Prefab/Enemy/Monkey"));
        prefabs.Add(7, Resources.Load<Dog>("Prefab/Enemy/JingCu"));
        //prefabs.Add(8, Resources.Load<XiaoHuoGuai>("Prefab/Enemy/HuoShu"));
    }

    void LoadMonster()
    {
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
