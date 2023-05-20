using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Networking.Core;
using System.Linq;
using Khan_Shared.Networking;

namespace Networking.Services
{
    public class PlayerPositionService: IInitializable, ILateDisposable
    {
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly IPlayersController m_playersController;

        public void Initialize()
        {
            GameServer.onServerTick += onTick;
        }

        public void LateDispose()
        {
            GameServer.onServerTick -= onTick;
        }

        public void onTick(int tick)
        {
            PlayerRefrenceObject[] players = m_playersController.getPlayers();

            foreach (var player in players)
            {
                foreach (var otherplayer in players.Where(p => p._connectionId != player._connectionId))
                {
                    Vector2 faceRotation = otherplayer._playerPositionBehaviour.FaceRotation;
                    object[] data = new object[]
                    {
                        (ushort)otherplayer._connectionId,
                        (float)otherplayer._gameObject.transform.position.x,
                        (float)otherplayer._gameObject.transform.position.y,
                        (float)otherplayer._gameObject.transform.position.z,
                        (float)faceRotation.x,
                        (float)faceRotation.y,
                    };
                    Message msg = new Message(MessageTypes.PositionData, data, MessagePriorities.medium);
                    m_messagePublisher.PublishMessage(msg, player._connectionId);
                }
            }
        }
    }
}
