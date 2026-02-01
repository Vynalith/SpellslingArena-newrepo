using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    // If your old placer script exists, drag it in here:
    [SerializeField] private MonoBehaviour legacyPlacer;

    // These names must match the functions on the legacy placer if you use SendMessage fallback.
    // If you know the actual type, we can replace this with direct method calls.

    public void SetPosition(Vector3 offsetTotal)
    {
        // Prefer direct transform logic if your placer just needs positioning:
        // transform.position = offsetTotal;

        // If the placer already has logic, forward it:
        if (legacyPlacer != null)
            legacyPlacer.SendMessage("SetPosition", offsetTotal, SendMessageOptions.DontRequireReceiver);
        else
            transform.position = offsetTotal;
    }

    public void CreateRoom(int roomId)
    {
        if (legacyPlacer != null)
            legacyPlacer.SendMessage("CreateRoom", roomId, SendMessageOptions.DontRequireReceiver);
    }

    public void CreateBossRoom()
    {
        if (legacyPlacer != null)
            legacyPlacer.SendMessage("CreateBossRoom", SendMessageOptions.DontRequireReceiver);
    }

    public void GetRoomCount(int count)
    {
        if (legacyPlacer != null)
            legacyPlacer.SendMessage("GetRoomCount", count, SendMessageOptions.DontRequireReceiver);
    }
}
