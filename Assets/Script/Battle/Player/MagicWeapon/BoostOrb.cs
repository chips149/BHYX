using UnityEngine;

public class BoostOrb : MonoBehaviour
{
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 180f;
    public float flySpeed = 8f;

    private Transform orbitCenter;
    private float baseAngle;
    private int totalCount;
    private float currentAngle;
    private bool isFlying;

    public System.Action OnOrbDestroyed;

    public void SetBaseAngle(float angle) => baseAngle = angle;
    public void SetTotalCount(int count) => totalCount = count;

    public static BoostOrb Create(Transform orbitCenter, Vector3 landPos, float baseAngle, int totalCount)
    {
        var prefab = Resources.Load<BoostOrb>("Prefab/Bullet/BoostOrb");
        var orb = Instantiate(prefab);
        orb.orbitCenter = orbitCenter;
        orb.baseAngle = baseAngle;
        orb.totalCount = totalCount;
        orb.currentAngle = baseAngle;
        orb.isFlying = true;
        orb.transform.position = landPos;
        return orb;
    }

    private void Update()
    {
        if (orbitCenter == null)
        {
            Destroy(gameObject);
            return;
        }

        if (isFlying)
        {
            transform.position = Vector3.MoveTowards(transform.position, orbitCenter.position, flySpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, orbitCenter.position) < 0.1f)
                isFlying = false;
            return;
        }

        currentAngle += orbitSpeed * Time.deltaTime;
        float angleOffset = baseAngle * Mathf.Deg2Rad;
        float finalRad = currentAngle * Mathf.Deg2Rad + angleOffset;

        Vector3 offset = new Vector3(
            Mathf.Cos(finalRad) * orbitRadius, 0.5f, Mathf.Sin(finalRad) * orbitRadius
        );

        transform.position = orbitCenter.position + offset;
    }

    private void OnDestroy()
    {
        OnOrbDestroyed?.Invoke();
    }
}
