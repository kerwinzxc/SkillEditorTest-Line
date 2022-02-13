namespace SuperCLine.ActionEngine
{
    public static class UnitFactory
    {
        public static PlayerBase NewPlayer(bool is2D)
        {
            if (is2D)
                return new Player2D();
            return new Player();
        }
        
        public static MonsterBase NewMonster(bool is2D)
        {
            if (is2D)
                return new Monster2D();
            return new Monster();
        }
    } 
}