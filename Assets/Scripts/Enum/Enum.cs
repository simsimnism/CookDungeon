// 방의 방향
public enum Orientation
{
    north,
    east,
    south,
    west,
    none
}

// 플레이어의 방향
public enum PlayerOrientation
{
    up,
    down,
    left,
    right,
    none
}

// 게임 상태
public enum GameState
{
    title, // 타이틀 화면
    gameStarted, // 게임 최초 시작
    playingLevel, // 레벨 진행 중
    bossStage, // 보스 방
    levelCompleted, // 레벨 클리어 (다음 레벨로 넘어가야 함.)
    gamePaused, // 게임 정지
    dungeonMapOverview, // 던전 맵 보기
    restartGame // 게임 재시작
}