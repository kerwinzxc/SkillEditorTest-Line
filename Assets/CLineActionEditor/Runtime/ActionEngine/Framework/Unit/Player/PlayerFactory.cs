namespace SuperCLine.ActionEngine
{
    public static class PlayerFactory
    {
        public static PlayerBase NewPlayer(bool is2DPlayer)
        {
            if (is2DPlayer)
                return new Player2D();
            return new Player();
        }
    } 
}