using Framework.Gameplay;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IBeHit
{
    public float maxHp = 50;
    public float currentHp;
    public TextMeshProUGUI playerHealthText;
    public PlayerManager pm;

    void Start()
    {
    }

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
            pm.container.RemoveEffect<DotBuff>();
            return;
        }


        pm.container.Execute(data);

        TakeDamage(data.damage);
    }

    public void RemoveHp(RemoveHpData data)
    {
        pm.container.Execute(data);

        TakeDamage(data.damage);
    }


    private void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
        }

        UpdateHealthDisplay();
    }


    public void UpdateHealthDisplay()
    {
        playerHealthText.text = $" {currentHp}";
    }
}