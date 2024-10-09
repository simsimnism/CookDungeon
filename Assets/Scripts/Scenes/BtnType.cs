using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum BTNType
{
    New,
    Continue,
    Setting,
    Sound,
    Back,
    Credit,
    Quit,
    backMENU
}
public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup Main_BTN;
    public CanvasGroup Setting_BTN;
    public CanvasGroup Credit_BTN;
    public CanvasGroup StartMenu_BTN;
    public CanvasGroup Sound_BTN;
    public CanvasGroup Quit_BTN;
    void Start()
    {
        defaultScale = buttonScale.localScale;
    }

    bool isSound;
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.New:
                CanvasGroupON(StartMenu_BTN);
                CanvasGroupOFF(Main_BTN);
                Debug.Log("새게임");
                break;
            /*
            case BTNType.Continue:
                CanvasGroupON(StartMenu_BTN);
                CanvasGroupOFF(Main_BTN);
                Debug.Log("이어하기");
                break;
            */
            case BTNType.Setting:
                CanvasGroupON(Sound_BTN);
                CanvasGroupOFF(Main_BTN);
                Debug.Log("옵션");
                break;
            case BTNType.Sound:
                CanvasGroupON(Sound_BTN);
                CanvasGroupOFF(Setting_BTN);
                break;
            case BTNType.Back:
                CanvasGroupON(Main_BTN);
                CanvasGroupOFF(Setting_BTN);
                CanvasGroupOFF(Credit_BTN);
                CanvasGroupOFF(StartMenu_BTN);
                CanvasGroupOFF(Sound_BTN);
                Debug.Log("뒤로");
                break;
            case BTNType.Credit:
                CanvasGroupON(Credit_BTN);
                CanvasGroupOFF(Main_BTN);
                Debug.Log("크레딧");
                break; 
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("나가라~");
                break;
            case BTNType.backMENU:
                SceneManager.LoadScene("MAIN");
                break;
        }
    }
    public void CanvasGroupON(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void CanvasGroupOFF(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    //이미지 사이즈 커서 접근 시 일시적 확대
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}