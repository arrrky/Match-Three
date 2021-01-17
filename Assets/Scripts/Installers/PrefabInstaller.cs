using UnityEngine;
using Zenject;

public class PrefabInstaller : MonoInstaller
{
    [SerializeField] private GameObject fieldManagerPrefab;

    public override void InstallBindings()
    {
        GameObject fieldManager = Container.InstantiatePrefab(fieldManagerPrefab);
        Container.Bind<FieldManager>().FromComponentInNewPrefab(fieldManagerPrefab).AsSingle();
    }
}