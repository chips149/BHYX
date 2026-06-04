using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MonsterPanel : MonoBehaviour
{
    public MonsterConfig display;

    public Transform parent;

    [Header("Detail")]
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text traitText;
    public TMP_Text introText;

    private readonly List<MonsterDisplayCell> Cells = new();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {

        var prefab = Resources.Load<MonsterDisplayCell>(display.prefabPath);

        for (var i = 0; i < display.list.Count; i++)
        {
            var info = display.list[i];
            var cell = Instantiate(prefab, parent.transform);
            cell.id = i;
            cell.Initialize(this, info);
            Cells.Add(cell);
        }

        Display(Cells.First().id);
    }

    public void Display(int id)
    {
        var info = display.list[id];
        nameText.text = info.name;
        hpText.text = info.hp;
        traitText.text = info.trait;
        introText.text = info.intro;
    }
}
