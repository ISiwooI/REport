public class SuddenEventSample : SuddenEvent
{

    public override string name => "SampleName";

    public override string description => "SampleDescription";

    public override bool isPositive => false;

    public override bool isNegative => false;

    public override bool isAvoidable => true;

    public override int costMoney => 0;

    public override int costMental => throw new System.NotImplementedException();

    public override int costStemina => throw new System.NotImplementedException();

    public override int rewardMoney => throw new System.NotImplementedException();

    public override int rewardMental => throw new System.NotImplementedException();

    public override int rewardStemina => throw new System.NotImplementedException();

    public override int rewardExp => throw new System.NotImplementedException();

    public override int weightValue => throw new System.NotImplementedException();

    public override void AgreeEvent(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override SuddenEventKind GetKind()
    {
        throw new System.NotImplementedException();
    }

    public override void RejectEvent(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }
}