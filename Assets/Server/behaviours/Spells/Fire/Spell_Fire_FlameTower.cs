using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking.Services;
using Zenject;
using ConnectionId = System.Int32;
using Networking.Behaviours;

public class Spell_Fire_FlameTower : MonoBehaviour
{
    private readonly float m_startUpDuration = 5; //in seconds
    private readonly float m_fireDuration = 2.5f;
    private readonly float m_endDuration = 1.5f;

    private List<ConnectionId> m_playersInCollider = new List<ConnectionId>();

    [Inject] private readonly ISpellUtil_BasicTimer m_basicTimerUtil;
    [Inject] private readonly IPlayersController m_playersController;
    [Inject] private readonly ILoggerService m_logger;

    private void Awake()
    {
        m_basicTimerUtil.onFireEnd += OnFireEnd;
        m_basicTimerUtil.onEnd += OnEnd;
    }

    void OnEnable()
    {
        m_basicTimerUtil.start(m_startUpDuration, m_fireDuration, m_endDuration);
    }

    void OnFireEnd()
    {
        // damage
        m_logger.LogMessage($"boooooom");
        foreach (var player in m_playersInCollider)
        { 
            m_logger.LogMessage($"player {player} got hit by fire");
        }
    }

    void OnEnd()
    {
        // release to pool
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerRefrenceObject? player = m_playersController.getPlayer(collision.gameObject);

        if (player == null) return;
        if (m_playersInCollider.Exists(p => p == player.Value._connectionId)) return;

        m_playersInCollider.Add(player.Value._connectionId);
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerRefrenceObject? player = m_playersController.getPlayer(collision.gameObject);

        if (player == null) return;
        if (!m_playersInCollider.Exists(p => p == player.Value._connectionId)) return;

        m_playersInCollider.Remove(player.Value._connectionId);
    }

    public class Factory : PlaceholderFactory<Spell_Fire_FlameTower> { }
}
