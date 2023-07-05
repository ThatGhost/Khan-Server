using UnityEngine;
using UnityEngine.Serialization;

namespace Networking.Behaviours
{
    public class DebugStatBehaviour : MonoBehaviour, IDebugStatBehaviour
    {
        [Rename("KBs/s out")] public float kbsOut = 0;
        [Rename("Mgs/s out")] public float messagePS = 0;

        private float seconds = 0;
        private int bytesOut = 0;
        private int messagesOut = 0;

        private void Update()
        {
            seconds += Time.deltaTime;
            if (seconds >= 10)
            {
                seconds = 0;
                bytesOut = 0;
                messagesOut = 0;
            }
        }

        private void FixedUpdate()
        {
            kbsOut = (bytesOut / 1000f) / seconds;
            messagePS = messagesOut / seconds;
        }

        public void addBytesOut(int bytes)
        {
            bytesOut += bytes;
        }

        public void addMessagesOut(int messages)
        {
            messagesOut += messages;
        }
    }
}
