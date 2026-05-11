using System.Collections;
using UnityEngine;

public class XiaoHuoGuai : EnemyBase
{
    public ParticleSystem dieEffect;
    public ParticleSystem atkEffect;
    // movement
    private bool isMove;


    protected override void Move(float dt)
    {
        if (isMove)
        {
            transform.position += transform.forward * (speed * dt);
        }
    }

    public void Switch()
    {
        isMove = !isMove;
    }

    public void Atk()
    {
        atkEffect.Play();
    }


   public override void BeHit(BeHitData data)
       {
           base.BeHit(data);
           
           if (hp <= 0 )
           {
               var eff = Instantiate(dieEffect);
               var pos = transform.position;
               pos.y += 1;
               eff.transform.position = pos;
               eff.Play();
               Destroy(eff.gameObject, 0.4f);
           }
       }
}