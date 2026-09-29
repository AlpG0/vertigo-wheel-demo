namespace VertigoWheel.Rewards
{
    /// <summary>
    /// Wheel bir odulde durunca, odulun sonucunu bildirdigi taraf (oyun akisi).
    /// </summary>
    public interface IRewardSink // wheel bir odulde durunca, odulun sonucunu bildirdigi taraf (oyun akisi)
    {
        void Collect(RewardDefinition reward, int amount); 
        void Explode(); 
    }
}