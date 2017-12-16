namespace RoomGen.Enums
{
    public enum RoomType
    {
        Null = 0,
        Spawn = 1,
        Normal = 2,
        Key = 3,
        Boss = 4,
        Bonus = 5,
        Trap = 6
    }

    public enum Difficulty
    {
        Easy = 1,
        Normal = 2,
        Hard = 3
    }

    /// <summary>
    /// Dummy bonus items
    /// </summary>
    public enum BonusItem
    {
        Key = 0,
        Brain = 1,
        PigeonOfImmunity = 2
    }

    public enum Position
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Down = 3,
        Center = 4
    }

}
