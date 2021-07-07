using UnityEngine;

class PlayerBulletSpawner : BulletSpawner
{
    public InputController ControlsSwitch;

    protected override bool IsShot { get => Input.GetButtonDown("Fire") || (ControlsSwitch.IsMouse && Input.GetButtonDown("MouseFire")); }

    new void Awake()
    {
        base.Awake();
        ObjectInitializer.OnShotEventDictionary[Prefab.layer] += (_, shotObject) => MainMenu.Score += ObjectInitializer.ScoreDictionary[shotObject.layer](shotObject);
    }
}