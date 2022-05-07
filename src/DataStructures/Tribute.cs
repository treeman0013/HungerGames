using Vintagestory.API.Server;

namespace HungerGames
{
    public class Tribute
    {
        public string playerUID;
        public bool isConnected;
        public bool isAlive;

        public Tribute()
        {
            isConnected = false;
            isAlive = true;
        }
    }
}