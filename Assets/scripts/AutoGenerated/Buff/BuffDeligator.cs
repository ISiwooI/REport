public class BuffDeligator
{
    public Buff GetBuff(BuffKind buffKind)
    {
        switch (buffKind)
        {

            case BuffKind.emotionalWellBeing:
                return new BEmotionalWellBeing();
            case BuffKind.healthyBody:
                return new BHealthyBody();
            case BuffKind.hungry:
                return new BHungry();
            case BuffKind.musclePain:
                return new BMusclePain();
            case BuffKind.report:
                return new BReport();
            case BuffKind.tierd:
                return new BTired();
            case BuffKind.awakening:
                return new BAwakening();
            default:
                return null;
        }
    }
}