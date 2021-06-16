using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public class ScreenInstanceBase : ScreenInstance
{
    private static RectTransform _MsgBoxBackgroundPrefab;
    private static MessageBoxWnd _Wnd_MessageBoxWndPrefab;


    protected override void Awake()
    {
        base.Awake();

        if (!_MsgBoxBackgroundPrefab)
        {
            _MsgBoxBackgroundPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "Panel_MessageBoxBackground",
                "Prefabs/UI/ClosableWnd/MessageBoxWnd/Panel_MessageBoxBackground").transform as RectTransform;

            _Wnd_MessageBoxWndPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "Wnd_MessageBoxWnd", "Prefabs/UI/ClosableWnd/MessageBoxWnd/Wnd_MessageBox").GetComponent<MessageBoxWnd>();

        }
    }

    // 메시지 박스를 생성합니다.
    /// - titleText : 메시지 박스 타이틀 문자열을 전달합니다.
    /// - message : 메시지 박스의 내용을 전달합니다.
    /// - useBackground : 배경 사용 여부를 전달합니다.
    /// - useButton : 사용하는 버튼들을 전달합니다.
    public MessageBoxWnd CreateMessageBox(
        string titleText,
        bool useBackground,
        params MessageBoxButton[] useButton)
    {
        RectTransform msgBoxBackground = null;

        if (useBackground)
        {
            msgBoxBackground = Instantiate(_MsgBoxBackgroundPrefab, m_Panel_WndParent);
        }

        // 메시지 박스 생성
        MessageBoxWnd newMessageBox = CreateWnd(_Wnd_MessageBoxWndPrefab, false, InputMode.UIOnly, true) as MessageBoxWnd;

        // 메시지 박스 배경 객체 설정
        newMessageBox.m_MsgBoxBackground = msgBoxBackground;

        // 메시지 박스 초기화
        newMessageBox.InitializeMessageBox(titleText, useButton);

        // 생성한 메시지 박스 창 반환
        return newMessageBox;
    }
}
