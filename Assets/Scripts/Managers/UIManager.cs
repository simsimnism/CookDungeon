using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 중복 생성 방지
        if (GetPopup<T>() != null)
        {
            Debug.Log($"{typeof(T).Name} 팝업이 이미 열려 있습니다.");
            return null;
        }

        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    // 특정 타입의 팝업이 열려 있는지 확인하는 메서드
    public T GetPopup<T>() where T : UI_Popup
    {
        foreach (UI_Popup popup in _popupStack)
        {
            if (popup is T)
                return popup as T;
        }
        return null;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Contains(popup))
        {
            Stack<UI_Popup> tempStack = new Stack<UI_Popup>();

            while (_popupStack.Peek() != popup)
            {
                tempStack.Push(_popupStack.Pop());
            }

            UI_Popup targetPopup = _popupStack.Pop();
            targetPopup.gameObject.SetActive(false);  // 팝업을 UI에서 비활성화
            Managers.Resource.Destroy(targetPopup.gameObject);  // 이후 파괴
            _order--;

            while (tempStack.Count > 0)
            {
                _popupStack.Push(tempStack.Pop());
            }

            Debug.Log($"{popup.name} 팝업이 닫혔습니다.");
        }
        else
        {
            Debug.Log("스택에 해당 팝업이 없습니다.");
        }
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        popup.gameObject.SetActive(false);  // 팝업 비활성화 추가
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
