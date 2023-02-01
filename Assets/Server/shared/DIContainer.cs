using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking.Services;

namespace Networking.Shared
{
    public class DIContainer : MonoBehaviour
    {
        private static DIContainer _instance;
        public static DIContainer Instance
        {
            get
            {
                return _instance;
            }
        }


        private Dictionary<string, MonoBehaviour> m_behaviours;
        private Dictionary<string, ServiceBase> m_services;
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<DIContainer>();
            m_behaviours = new Dictionary<string, MonoBehaviour>()
            {
                { typeof(BehaviourBase).ToString(), FindObjectOfType<BehaviourBase>() }
            };

            m_services = new Dictionary<string, ServiceBase>()
            {
                { typeof(SetupService).ToString(), new SetupService() }
            };

            foreach (var server in m_services)
            {
                server.Value.Init();
            }
        }

        public T getService<T>() where T : ServiceBase
        {
            return (T)m_services[typeof(T).ToString()];
        }

        public T getBehaviour<T>() where T : MonoBehaviour
        {
            return (T)m_behaviours[typeof(T).ToString()];
        }
    }
}