using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class ServerConnector
    {
     
        private int _myReliableChannelId;

        public int GetChannelId()
        {
            return _myReliableChannelId;
        }

        public void Init()
        {
            NetworkTransport.Init();
        }


    }
}