using System;

public class EventHandle
{
    // 방이 변경 될 때 이벤트 호출
    public static event Action<RoomChangeEvent> OnRoomChange;

    public static void CallRoomChangeEvent(Room room)
    {
        OnRoomChange?.Invoke(new RoomChangeEvent() { room = room});
    }

    // 방에 몬스터가 없을 때 이벤트 호출
    public static event Action<RoomMonsterClear> OnRoomMonsterClear;

    public static void CallRoomMonsterClearEvent(Room room)
    {
        OnRoomMonsterClear?.Invoke(new RoomMonsterClear() { room = room });
    }
}



public class RoomChangeEvent : EventArgs
{
    public Room room;
}

public class RoomMonsterClear : EventArgs
{
    public Room room;
}