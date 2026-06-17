using UnityEngine;
using System.Collections;

[CardProperty(19, "水盾", "UI/Card/Icon/WaterShield", "每40秒刷新水盾\n攻击力-2\n子弹回复速度+15%",isSkillCard = true)]

public class WaterShield : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        var ph = GameState.Pm.playerHealth;
        
        pp.damage -= 2;
        pp.bulletReloadTime *= 0.85f;
        GameState.Pm.UpdateProperty();

        ph.shieldCardLevel++;
        int bonus = 10 + (ph.shieldCardLevel - 1) * 5;

        if (ph.shieldActive)
        {
            ph.RemoveShield();
        }
        CreateShieldEffect(ph);
        ph.ApplyShield(bonus);

        ph.StartCoroutine(ShieldRoutine(ph));
    }

    public override void OnReplay()
    {
        var ph = GameState.Pm.playerHealth;
        ph.shieldCardLevel = SaveData.Instance.shieldCardLevel;

        if (ph.shieldCardLevel > 0)
        {
            int bonus = 10 + (ph.shieldCardLevel - 1) * 5;
            CreateShieldEffect(ph);
            ph.ApplyShield(bonus);
        }

        ph.StartCoroutine(ShieldRoutine(ph));
    }

    private static void CreateShieldEffect(PlayerHealth ph)
    {
        if (ph.shieldFx != null) return;

        var prefab = Resources.Load<WaterShieldEffect>("Prefab/Item/WaterShield");
        var playerTrans = GameState.Pm.player.transform;
        
        var effect = Object.Instantiate(prefab, playerTrans);
        effect.transform.localPosition = new Vector3(0, 0, -0.5f);
        ph.shieldFx = effect;
    }
    
    public static IEnumerator ShieldRoutine(PlayerHealth ph)
    {
        while (true)
        {
            yield return new WaitForSeconds(40f);
            if (!ph.shieldActive && ph.shieldCardLevel > 0)
            {
                int bonus = 10 + (ph.shieldCardLevel - 1) * 5;
                ph.ApplyShield(bonus);
            }
        }
    }
}