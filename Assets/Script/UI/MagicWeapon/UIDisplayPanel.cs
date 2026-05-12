using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIDisplayPanel : MonoBehaviour
{
    public ConfigAsset displayAsset;

    [Header("Reference")] public Image icon;
    public Text nameText;
    public Text description;
    public Text detail;

    public Transform parent;

    public int selectedID = -1;

    public readonly List<DisplayCell> Cells = new();
    
    public UnityEvent onAfterInitialize;
    
    private void Start()
    {
        Initialize();
        
        onAfterInitialize?.Invoke();
    }



    private void Initialize()
    {
        var prefab = Resources.Load<DisplayCell>(displayAsset.prefabPath);

        for (var i = 0; i < displayAsset.list.Count; i++)
        {
            var info = displayAsset.list[i];
            var behavior = Instantiate(prefab, parent.transform);
            behavior.id = i;
            behavior.Initialize(this, info);
            Cells.Add(behavior);
        }

        Display(Cells.First());
    }

    public void Display(DisplayCell cell)
    {
        selectedID = cell.id;
        icon.sprite = cell.displayIcon;
        nameText.text = cell.mwName;
        description.text = cell.description;
        detail.text = cell.detail;
    }
}
