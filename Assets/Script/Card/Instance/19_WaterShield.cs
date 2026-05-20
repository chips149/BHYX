using System.Resources;
using UnityEngine;
using System.Collections;

[CardProperty(19, "水盾", "", "每40秒获得一层护盾（上限1层）,攻击力-2,子弹回复速度+15%")]

public class WaterShield : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        var ph = GameState.Pm.playerHealth;
        
        pp.damage -= 2;
        pp.bulletReloadTime *= 0.85f;
        GameState.Pm.UpdateProperty();

        ph.shield = 1;
        CreateShieldEffect(ph);
        ph.StartCoroutine(ShieldRoutine(ph));
    }

    public override void OnReplay()
    {
        var ph = GameState.Pm.playerHealth;
        ph.shield = SaveData.Instance.shield;
        if (ph.shield > 0)
        {
            CreateShieldEffect(ph);
        }
        ph.StartCoroutine(ShieldRoutine(ph));
    }

    private static void CreateShieldEffect(PlayerHealth ph)
    {
        var prefab = Resources.Load<WaterShieldEffect>("Prefab/Item/WaterShield");
        var playerTrans = GameState.Pm.player.transform;
        var point = playerTrans.Find("WaterShieldPoint");
        
        var effect = Object.Instantiate(prefab, point.position, point.rotation, point);
        ph.shieldFx = effect;
        effect.ShowShield();
    }
    
    public static IEnumerator ShieldRoutine(PlayerHealth ph)
    {
        while (true)
        {
            yield return new WaitForSeconds(40f);
            if (ph.shield < 1)
            {
                ph.shield = 1;
                ph.shieldFx?.ShowShield(); 
            }
        }
    }
}