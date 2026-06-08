using TMPro;
using UnityEngine;

public class WeaponLevelReset : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text currentLevelText;
    public TMP_Text upgradePreviewText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            MagicWeaponLevelUpSystem.ResetAllLevels();

            if (currentLevelText != null)
                currentLevelText.text = "1";
            if (upgradePreviewText != null)
                upgradePreviewText.text = "2";

            Debug.Log("已重置所有法宝等级");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CoinSystem.AddCoin(1000);
            CoinSystem.CommitSessionCoins();

            if (coinText != null)
                coinText.text = $"{CoinSystem.GetCoin()}";

            Debug.Log($"已增加 1000 金币，当前金币：{CoinSystem.GetCoin()}");
        }
    }

    public void ResetAllLevels()
    {
        MagicWeaponLevelUpSystem.ResetAllLevels();
    }
}
