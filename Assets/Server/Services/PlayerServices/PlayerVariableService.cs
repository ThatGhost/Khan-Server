using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Khan_Shared.Networking;
using static Server.Services.IPlayerVariableService;
using ConnectionId = System.Int32;
using System;
using UnityEditor.MemoryProfiler;

namespace Server.Services
{
    public class PlayerVariableService : IPlayerVariableService, IInitializable, IDisposable
    {
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly IClockService m_clockService;
        [Inject] private readonly SignalBus m_signalBus;

        private ConnectionId m_connectionId;

        private int m_hp = 100;
        private int m_maxHp = 100;
        private int m_maxMana = 100;
        private int m_mana = 100;

        public int Mana
        {
            get => m_mana;
        }
        public int Health
        {
            get => m_hp;
        }
        public int MaxMana
        {
            get => m_maxMana;
            set => m_maxMana = value;
        }
        public int MaxHealth
        {
            get => m_maxMana;
            set => m_maxMana = value;
        }

        public void Initialize()
        {
            m_signalBus.Subscribe<OnHealthSignal>(x => addHp(x.amount, x.connectionId));
            m_signalBus.Subscribe<OnManaSignal>(x => addMana(x.amount, x.connectionId));
        }

        public void Dispose()
        {
            m_signalBus.TryUnsubscribe<OnHealthSignal>(x => addHp(x.amount, x.connectionId));
            m_signalBus.TryUnsubscribe<OnManaSignal>(x => addMana(x.amount, x.connectionId));
        }

        public void setup(ConnectionId connectionId)
        {
            m_connectionId = connectionId;
            m_clockService.onClockTick = onClockTick;
            m_clockService.StartClock(0.5f);
        }

        private void addHp(int amount, int connectionId)
        {
            if (connectionId == m_connectionId) addHealth(amount);
        }

        private void addMana(int amount, int connectionId)
        {
            if (connectionId != m_connectionId) return;

            m_mana += amount;
            if (m_mana <= 0) m_mana = 0;
            if (m_mana >= m_maxHp) m_mana = m_maxMana;

            sendManaMessage();
        }

        private void sendHpMessage()
        {
            Message msg = new Message(MessageTypes.PlayerHealthUpdate, new object[]
            {
                (ushort) m_connectionId,
                (ushort) m_hp,
            } ,MessagePriorities.medium);
            m_messagePublisher.PublishGlobalMessage(msg);
        }

        private void sendManaMessage()
        {
            Message msg = new Message(MessageTypes.PlayerManaUpdate, new object[]
            {
                (ushort) m_mana,
            }, MessagePriorities.medium);
            m_messagePublisher.PublishMessage(msg, m_connectionId);
        }

        private void onClockTick()
        {
            addMana(5, m_connectionId);
        }

        public void addHealth(int amount)
        {
            m_hp += amount;
            if (m_hp <= 0)
            {
                m_hp = 0;
                m_signalBus.Fire(new OnDeathSignal() { connectionId = m_connectionId });
            }
            if (m_hp >= m_maxHp) m_hp = m_maxHp;

            sendHpMessage();
        }
    }
}
