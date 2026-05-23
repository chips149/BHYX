using Framework.Gameplay;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IBeHit
{
    public float maxHp = 50;
    public float currentHp;
    public TextMeshProUGUI playerHealthText;
    public PlayerManager pm;
    public GameObject firePrefab;

    public bool shieldActive;           
    public float shieldBonus;          
    public int shieldCardLevel;        
    public WaterShieldEffect shieldFx;

    public void Initialize(PlayerManager pm)
    {
        this.pm = pm;
        var textObj = GameObject.Find("PlayerHealthText");

        playerHealthText = textObj.GetComponent<TextMeshProUGUI>();

        currentHp = maxHp;
        UpdateHealthDisplay();
    }

    public void BeHit(BeHitData data)
    {
        if (data.from == "player")
        {
            firePrefab.SetActive(false);
            SoundManager.StopLoop();
            pm.container.RemoveEffect<DotBuff>();
            return;
        }


        pm.container.Execute(data);

        TakeDamage(data.damage);
    }

    public void RemoveHp(RemoveHpData data)
    {
        firePrefab.SetActive(true);
        SoundManager.PlayLoop("Audio/SFX/Monster/FenceBurn");
        pm.container.Execute(data);

        TakeDamage(data.damage);
    }
    
    public void ApplyShield(int bonus)
    {
        shieldActive = true;
        shieldBonus = bonus;
        maxHp += bonus;
        currentHp += bonus;
        shieldFx?.ShowShield();
        UpdateHealthDisplay();
    }

    public void RemoveShield()
    {
        if (!shieldActive) return;
        shieldActive = false;
        maxHp -= shieldBonus;
        if (currentHp > maxHp)
            currentHp = maxHp;
        shieldBonus = 0;
        shieldFx?.HideShield();
        shieldFx = null;
        UpdateHealthDisplay();
    }

    private void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (shieldActive && currentHp < maxHp - shieldBonus)
        {
            RemoveShield();
        }

        if (currentHp <= 0)
        {
            currentHp = 0;
            GameUIManager.instance.Lose();
        }

        UpdateHealthDisplay();
    }


    public void UpdateHealthDisplay()
    {
        playerHealthText.text = $" {currentHp}";
    }
}