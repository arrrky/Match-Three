using System.ComponentModel;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ScoreManager>().AsSingle();
    }
}