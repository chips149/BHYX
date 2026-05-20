using UnityEngine;

public class WaterTornadoProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 3f;

    private PlayerManager pm;
    private float timer;
    private bool isDisappearing;

    public ParticleSystem spawnFx;
    public ParticleSystem sustainFx;
    public ParticleSystem disappearFx;

    public void Init(PlayerManager pm)
    {
        this.pm = pm;
    }

    private void Start()
    {
        spawnFx.Play();
        sustainFx.gameObject.SetActive(false);
        disappearFx.gameObject.SetActive(false);

        if (spawnFx)
            Invoke(nameof(PlaySustain), spawnFx.main.duration);
        else
            PlaySustain();
    }

    private void PlaySustain()
    {
        sustainFx.gameObject.SetActive(true);
        sustainFx.Play();
    }

    private void Update()
    {
        if (isDisappearing) return;

        transform.position += Vector3.forward * (speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer > lifeTime) StartDisappear();
    }

    private void StartDisappear()
    {
        isDisappearing = true;

        sustainFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        disappearFx.gameObject.SetActive(true);
        disappearFx.Play();
        Destroy(gameObject, disappearFx.main.duration);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IBeHit>(out var hit)) return;

        hit.BeHit(new BeHitData
        {
            damage = Mathf.RoundToInt(pm.baseProperty.damage * 0.2f),
            from = "player",
            attacker = pm.container,
        });
    }
}