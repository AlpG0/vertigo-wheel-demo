namespace VertigoWheel.Data // namespace kullanmak,"bu sinif projenin bu bolumune aittir" demektir. Bu sayede siniflarin birbirleriyle karismasini engelleriz.
{
    /// <summary>
    /// Bir wheel diliminin odul turunu belirtir.
    /// </summary>
    public enum RewardType //Bu degisken sadece su belirli degerleri alabilir. Bu degerler enum ile tanimlanmistir.
    {
        Bomb,
        Currency,
        Item
    }
}