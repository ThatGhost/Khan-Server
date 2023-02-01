using System;
using Khan_Shared.Networking;
using Networking.Services;

public class InitializerEntryPoint : EntryPointBase
{
    private SetupService m_SetupService;
    protected override void Initialize()
    {
        // Message registering
        p_messages.Add(new MessageFunctionPair(MessageTypes.HandShake, handShake));

        // Services setup
        m_SetupService = DIContainer.Instance.getService<SetupService>();
    }

    private void handShake(object[] data, int conn)
    {
        m_SetupService.foo((int)data[0], (int)data[1], conn);
    }
}