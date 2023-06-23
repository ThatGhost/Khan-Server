using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Khan_Shared.Networking;
using static Networking.Services.IPlayerVariableService;
using ConnectionId = System.Int32;

namespace Networking.Services
{
    public class PlayerVariableService : IPlayerVariableService
    {
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly IClockService m_clockService;

        private ConnectionId m_connectionId;
        private OnDeath m_onDeath;

        private int m_hp = 100;
        private int m_maxHp = 100;
        private int m_maxMana = 100;
        private int m_mana = 100;

        public OnDeath onDeath
        {
            get
            {
                return m_onDeath;
            }
            set
            {
                m_onDeath += value;
            }
        }
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

        public void setup(ConnectionId connectionId)
        {
            m_connectionId = connectionId;
            m_clockService.onClockTick = onClockTick;
            m_clockService.StartClock(0.5f);
        }

        public void addHp(int amount)
        {
            if (m_hp == m_maxHp && amount > 0) return;

            m_hp += amount;
            if (m_hp <= 0) {
                m_hp = 0;
                if (m_onDeath != null)
                    m_onDeath.Invoke(m_connectionId);
            }
            if(m_hp >= m_maxHp) m_hp = m_maxHp;

            sendHpMessage();
        }

        public void addMana(int amount)
        {
            if (m_mana == m_maxMana && amount > 0) return;

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
            addMana(5);
        }
    }
}
