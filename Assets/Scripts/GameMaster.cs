using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class GameMaster : Singelton<GameMaster>
    {
        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}