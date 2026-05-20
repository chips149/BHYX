using UnityEngine;

[CardProperty(21, "水龙卷", "", "怪物死亡后30%概率向后生成水龙卷（攻击力20%伤害），暴击率+10%，子弹大小+15%")]
public class WaterTornado : CardData
{
    public static bool hasWaterTornado;

    private static WaterTornadoProjectile prefab;

    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.critRate += 10 * pp.critRateCorrection;
        pp.bulletScale += 0.15f;
        GameState.Pm.UpdateProperty();

        hasWaterTornado = true;
    }

    public override void OnReplay()
    {
        hasWaterTornado = true;
    }

    public static void OnEnemyDeath(Vector3 enemyPos)
    {
        if (!hasWaterTornado) return;
        if (Random.value > 0.3f) return;
        
        prefab ??= Resources.Load<WaterTornadoProjectile>("Prefab/Bullet/WaterTornado");
        
        var go = Object.Instantiate(prefab, enemyPos, Quaternion.identity);
        go.Init(GameState.Pm);
    }
}
