using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


// 只管显示
public abstract class AimHandle
{
    public Action<Vector3> onAimEnd;
    public abstract void Aiming(Vector3 aimPos);

    public abstract void End();
}

public class DefaultAimHandle : AimHandle
{
    private readonly GameObject aimObject;

    private const float SCALE_SPEED = 0.8f;

    private float t;
    
    private Vector3 aimPos;

    public DefaultAimHandle()
    {
        var prefab = Resources.Load<GameObject>("Prefab/AimPrefab");
        aimObject = Object.Instantiate(prefab);
        aimObject.SetActive(false);
    }

    public override void Aiming(Vector3 aimPos)
    {
        aimObject.SetActive(true);

        var maxScale = GameState.Pm.baseProperty.maxSpread;
        var minScale = GameState.Pm.baseProperty.minSpread;
        t += Time.deltaTime * SCALE_SPEED;
        var scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(t, 1));

        aimObject.transform.localScale = Vector3.one * scale;
        aimObject.transform.position = aimPos;
        
        this.aimPos =  aimPos;
    }

    public override void End()
    {
        aimObject.SetActive(false);

        var maxScale = GameState.Pm.baseProperty.maxSpread;
        var minScale = GameState.Pm.baseProperty.minSpread;
        var currentSpread = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(t, 1));
        var randomAngle = Random.Range(0, Mathf.PI * 2);
        var randomRadius = Random.value * currentSpread;
        var offset = new Vector3(Mathf.Cos(randomAngle) * randomRadius, 0, Mathf.Sin(randomAngle) * randomRadius);

        onAimEnd?.Invoke(aimPos + offset);
    }
}