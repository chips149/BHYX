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
    public float shieldRemaining;       
    public int shieldCardLevel;        
    public WaterShieldEffect shieldFx;

    [Header("护盾UI")]
    public GameObject shieldUIRoot;
    public TextMeshProUGUI shieldText;

    public void Initialize(PlayerManager pm)
    {
        this.pm = pm;
        var textObj = GameObject.Find("PlayerHealthText");

        playerHealthText = textObj.GetComponent<TextMeshProUGUI>();

        currentHp = maxHp;
        shieldUIRoot?.SetActive(false);
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
        shieldRemaining = bonus;
        shieldFx?.ShowShield();
        UpdateShieldDisplay();
    }

    public void RemoveShield()
    {
        if (!shieldActive) return;
        shieldActive = false;
        shieldBonus = 0;
        shieldRemaining = 0;
        shieldFx?.HideShield();
        shieldUIRoot?.SetActive(false);
        UpdateShieldDisplay();
    }

    private void TakeDamage(float damage)
    {
        if (shieldActive && shieldRemaining > 0)
        {
            if (damage < shieldRemaining)
            {
                shieldRemaining -= damage;
                shieldFx?.OnHit();
                UpdateShieldDisplay();
            }
            else
            {
                damage -= shieldRemaining;
                shieldRemaining = 0;
                RemoveShield();
                currentHp -= damage;
            }
        }
        else
        {
            currentHp -= damage;
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

    private void UpdateShieldDisplay()
    {
        if (shieldUIRoot != null)
            shieldUIRoot.SetActive(shieldActive && shieldRemaining > 0);
        if (shieldText != null)
            shieldText.text = $"{(int)shieldRemaining}";
    }
}