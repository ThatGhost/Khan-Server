using UnityEngine;
using Zenject;
using Networking.Core;
using Networking.Services;

public class ServerInstaller : Installer<ServerInstaller>
{
    public override void InstallBindings()
    {
    }
}

/*
 * Bind<> for basic binding
 * BindExicutionOrder<> for order dependent
 * BindInterfacec...<> for multiple interfaces to 1 obj
 * 
 * To<> for basic binding
 * WhenInjectedInto<> for utils mostly
 * WithArguments<> for constructors
 * 
 * AsSingle() for basic 1-1 binding
 * AsTransient() new obj every inject
 * AsCached() for chaches
 * 
 * https://github.com/modesttree/Zenject/blob/master/Documentation/CheatSheet.md
 */