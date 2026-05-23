using Framework.Gameplay;

public class DotBuff : GameplayEffect, IGameplayEvent<FrameData>
{
    private const float Period = 0.5f;
    private int count;
    private float t;


    public override void OnRefresh()
    {
        count = 0;
    }

    public void Execute(FrameData data)
    {
        t += data.dt;
        if (t > Period)
        {
            t -= Period;
            count++;
            data.beHit.RemoveHp(new RemoveHpData
            {
                damage = 1
            });
        }

        if (count > 10)
        {
            finish = true;
        }
    }
}