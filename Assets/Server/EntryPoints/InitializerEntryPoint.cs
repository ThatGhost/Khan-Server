using System;
using Khan_Shared.Networking;
using Networking.Services;
using Networking.Shared;
using Zenject;

public class InitializerEntryPoint : EntryPointBase
{
    [Inject] private readonly IFooService m_SetupService;
    protected override void Initialize()
    {
        // Message registering
        p_messages.Add(new MessageFunctionPair(MessageTypes.HandShake, handShake));
    }

    private void handShake(object[] data, int conn)
    {
        m_SetupService.Foo((int)data[0], (int)data[1], conn);
    }
}