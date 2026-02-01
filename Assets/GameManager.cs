using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Placers (assign in Inspector in order)")]
    [SerializeField] private RoomPlacer[] placers; // e.g., size 6

    [Header("Offsets")]
    [SerializeField] private Vector3 lightningLevel1;
    [SerializeField] private Vector3 mudLevel1;
    [SerializeField] private Vector3 cornerLevel1;

    [Header("Random Room Pool")]
    [Tooltip("Number of non-boss room variants. Random will be [0..nonBossRoomVariants-1].")]
    [SerializeField] private int nonBossRoomVariants = 8;

    public int RoomCount { get; private set; }
    private Vector3 offsetTotal;

    private void Start()
    {
        RoomCount = 0;
        offsetTotal = Vector3.zero;

        if (placers == null || placers.Length == 0)
        {
            Debug.LogError("GameManager: No placers assigned.");
            return;
        }

        for (int i = 0; i < placers.Length; i++)
        {
            RoomPlacer placer = placers[i];
            if (placer == null)
            {
                Debug.LogWarning($"GameManager: Placer at index {i} is null.");
                continue;
            }

            bool isLastPlacer = (i == placers.Length - 1);

            // Apply current offset (layout position)
            placer.SetPosition(offsetTotal);

            if (!isLastPlacer)
            {
                int randomRoom = Random.Range(0, nonBossRoomVariants);
                placer.CreateRoom(randomRoom);

                // Apply any special offset rules based on room id
                offsetTotal += GetSpecialOffset(randomRoom);
            }
            else
            {
                // ✅ Always boss on the last placer
                placer.CreateBossRoom();
            }
        }
    }

    private Vector3 GetSpecialOffset(int roomId)
    {
        // Keep your “special room types shift the layout” logic
        return roomId switch
        {
            5 => lightningLevel1,
            6 => mudLevel1,
            7 => cornerLevel1,
            _ => Vector3.zero
        };
    }

    public void IncreaseRoomCount() => RoomCount++;

    public void SendRoomCount(RoomPlacer receiver)
    {
        if (receiver == null) return;
        receiver.GetRoomCount(RoomCount);
    }
}
