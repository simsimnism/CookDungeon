using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 팝업의 함수를 관리하는 코드
public class UIPopupManager 
{
    //스킬 유아이 관련
    private SkillPopup _SkillPopup;
    private bool _isSkillOpen = false;

    //요리 유아이 관련
    private CookingPopup _cookingPopup;
    private bool _isCookingOpen = false;

    //게임오버 유아이 관련
    private GameEndPopup _gameEndPopup;
    private bool _isGameEndOpen = false;

    //게임 정지 시작 유아이 관련
    private GamePausePopup _gamePausePopup;
    private bool _isGamePauseOpen = false;

    //로딩중 유아이 관련
    private GameLoadingPopup _gameLoadingPopup;
    private bool _isGameLoadingOpen = false;

    //게임오버 유아이 관련
    private GameClearPopup _gameClearPopup;
    private bool _isgameClearOpen = false;

    //게임오버 유아이 관련
    private RoundInfoPopup _RoundInfoPopup;
    private bool _isRoundInfoOpen = false;


    //============================스킬 UI_Popup==============================
    //스킬 관련 팝업은 시간멈춤이 들어가 있음
    public void ToggleSkillUI()
    {
        if (_isSkillOpen)
        {
            CloseSkillUI();
        }
        else
        {
            OpenSkillUI();
        }
    }

    private void OpenSkillUI()
    {
        if (_SkillPopup == null)
        {
            _SkillPopup = Managers.UI.ShowPopupUI<SkillPopup>("SkillChoicePopup");
            Time.timeScale = 0;  // 게임 시간 멈춤
        }
        else
        {
            _SkillPopup.gameObject.SetActive(true);
        }

        if (_SkillPopup != null)
        {
            _isSkillOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    public void CloseSkillUI()
    {
        if (_SkillPopup != null)
        {
            _SkillPopup.gameObject.SetActive(false);
            _isSkillOpen = false;
            Time.timeScale = 1;  // 게임 시간 재개
        }
    }
    //========================================================================


    //============================요리 UI_Popup==============================
    //UI 창 파괴생성 기능 담고있음
    public void ToggleCooking()
    {
        if (_isCookingOpen)
        {
            CloseCooking();
        }
        else
        {
            OpenCooking();
        }
    }

    //요리UI를 여는 로직
    private void OpenCooking()
    {
        if (_cookingPopup == null)
        {
            _cookingPopup = Managers.UI.ShowPopupUI<CookingPopup>("CookingPopup");
        }
        else
        {
            _cookingPopup.gameObject.SetActive(true);
        }

        if (_cookingPopup != null)
        {
            _isCookingOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    //요리UI를 닫는 로직
    public void CloseCooking()
    {
        if (_cookingPopup != null)
        {
            Managers.UI.ClosePopupUI();
            _isCookingOpen = false;
            _cookingPopup = null;
        }
    }
    //========================================================================


    //==========================게임오버 UI_Popup==============================
    public void ToggleGameEndUI()
    {
        if (_isGameEndOpen)
        {
            CloseGameEndUI();
        }
        else
        {
            OpenGameEndUI();
        }
    }

    public void OpenGameEndUI()
    {
        if (_gameEndPopup == null)
        {
            _gameEndPopup = Managers.UI.ShowPopupUI<GameEndPopup>("GameOverPopup");
            Time.timeScale = 0;  // 게임 시간 멈춤
        }
        else
        {
            _gameEndPopup.gameObject.SetActive(true);
        }

        if (_gameEndPopup != null)
        {
            _isGameEndOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    public void CloseGameEndUI()
    {
        if (_gameEndPopup != null)
        {
            //해당 창을 파괴하는 함수
            Managers.UI.CloseAllPopupUI();
            _isGameEndOpen = false;

            //값도 널로 바꿔줘야 함 안그러면 널로 값이 바뀌었다고 판단 안함
            _gameEndPopup = null;
            Time.timeScale = 1;  // 게임 시간 재개
        }
    }

    //===========================================================================


    //===========================게임정지 Popup==================================
    //게임 정지 시작 팝업 이 팝업은 셋 액티브가 아닌 파괴 생성을 원칙으로 함 (게임시간을 멈추는 기능도 있음)
    public void TogglePauseUI()
    {
        if (_isGamePauseOpen)
        {
            CloseGamePauseUI();
        }
        else
        {
            OpenGamePauseUI();
        }
    }

    public void OpenGamePauseUI()
    {
        if (_gamePausePopup == null)
        {
            _gamePausePopup = Managers.UI.ShowPopupUI<GamePausePopup>("GamePausePopup");
            Time.timeScale = 0;  // 게임 시간 멈춤
        }
        else
        {
            _gameEndPopup.gameObject.SetActive(true);
        }

        if (_gamePausePopup != null)
        {
            _isGamePauseOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    public void CloseGamePauseUI()
    {
        if (_gamePausePopup != null)
        {
            Managers.UI.ClosePopupUI();
            _isGamePauseOpen = false;

            //값도 널로 바꿔줘야 함 안그러면 널로 값이 바뀌었다고 판단 안함
            _gamePausePopup = null;
            Time.timeScale = 1;  // 게임 시간 재개
        }
    }
    //===========================================================================


    //============================로딩중 Popup===================================
    //UI 창 파괴생성 기능 담고있음(게임 시간 정지는 들어가 있지 않음)
    public void ToggleGameLoading()
    {
        if (_isGameLoadingOpen)
        {
            CloseGameLoading();
        }
        else
        {
            OpenGameLoading();
        }
    }

    public void OpenGameLoading()
    {
        if (_gameLoadingPopup == null)
        {
            _gameLoadingPopup = Managers.UI.ShowPopupUI<GameLoadingPopup>("GameLoadingPopup");
        }
        else
        {
            _gameLoadingPopup.gameObject.SetActive(true);
        }

        if (_gameLoadingPopup != null)
        {
            _isGameLoadingOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    public void CloseGameLoading()
    {
        if (_gameLoadingPopup != null)
        {
            Managers.UI.ClosePopupUI();
            _isGameLoadingOpen = false;
            _gameLoadingPopup = null;
        }
    }
    //===========================================================================


    //=============================게임 클리어 UI================================
    public void ToggleGameClear()
    {
        if (_gameClearPopup)
        {
            CloseGameEndUI();
        }
        else
        {
            OpenGameEndUI();
        }
    }

    public void OpenGameClear()
    {
        if (_gameClearPopup == null)
        {
            _gameClearPopup = Managers.UI.ShowPopupUI<GameClearPopup>("GameClear");
            _gameClearPopup.gameObject.SetActive(true);
        }
        else
        {
            _gameClearPopup.gameObject.SetActive(true);
        }

        if (_gameClearPopup != null)
        {
            _isgameClearOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }


    public void CloseGameClear()
    {
        if (_gameClearPopup != null)
        {
            
            _isgameClearOpen = false;
            _gameClearPopup.gameObject.SetActive( false);

        }
    }
    //===========================================================================

    //=============================라운드 표시 UI================================
    public void ToggleRoundInfo()
    {
        if (_RoundInfoPopup)
        {
            CloseRoundInfo();
        }
        else
        {
            OpenRoundInfo();
        }
    }

    public void OpenRoundInfo()
    {
        if (_RoundInfoPopup == null)
        {
            _RoundInfoPopup = Managers.UI.ShowPopupUI<RoundInfoPopup>("RoundInfoPopup");
        }
        else
        {
            _RoundInfoPopup.gameObject.SetActive(true);
        }

        if (_RoundInfoPopup != null)
        {
            _isRoundInfoOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }


    public void CloseRoundInfo()
    {
        if (_RoundInfoPopup != null)
        {

            _isRoundInfoOpen = false;
            _RoundInfoPopup.gameObject.SetActive(false);

        }
    }
    //===========================================================================

    //모든 팝업을 닫는 코드 게임이 종료되는 코드에는 이걸 무조건 실행시켜줘야 하며 모든 팝업을 닫는 코드는 여기다 넣어주세요
    public void RealAllClosePopup()
    {
        CloseSkillUI();
        CloseCooking();
        CloseGameEndUI();
        CloseGamePauseUI();
        CloseGameLoading();
        CloseGameClear();
        CloseRoundInfo();
        Managers.Inventory.CloseInventory();

    }
}
