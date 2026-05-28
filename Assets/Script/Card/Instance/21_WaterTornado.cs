using UnityEngine;

[CardProperty(21, "水龙卷", "", "杀敌概率生成水龙卷\n暴击率+10%\n子弹大小+15%",isSkillCard = true)]
public class WaterTornado : CardData
{
    public static bool hasWaterTornado;

    private static WaterTornadoProjectile prefab;

    public override string detailText => "怪物死亡后30%会向后生成一个伤害为攻击力20%水龙卷\n暴击率+10%\n子弹大小+15%";

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
