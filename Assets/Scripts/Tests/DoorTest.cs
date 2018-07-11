using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DoorTest{

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator DoorAloneDoesNotMove() {

        var door = new GameObject().AddComponent<DoorScript>();

        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;

        bool movingDoor = door.moving;

        Assert.IsFalse(movingDoor);
    }


    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator DoorWithUnitNextToItDoesNotMove()
    {
        PlayerActionScript unit = new PlayerActionScript();
        var door = new GameObject().AddComponent<DoorScript>();
        unit.InteractWithItem(door);
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;

        bool movingDoor = door.moving;

        Assert.IsFalse(movingDoor);
    }
}
