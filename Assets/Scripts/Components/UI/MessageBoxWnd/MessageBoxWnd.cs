﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public sealed class MessageBoxWnd : ClosableWnd
{
    public event System.Action<ScreenInstance, MessageBoxWnd> onOkButtonClicked;
    public event System.Action<ScreenInstance, MessageBoxWnd> onCancelButtonClicked;

    [SerializeField] private Button _Button_Yes;
    [SerializeField] private Button _Button_Cancel;
    [SerializeField] private TextMeshProUGUI _Input_ItemCount;
    [HideInInspector] public RectTransform m_MsgBoxBackground;

    private void ButtonVisibility(Button button, MessageBoxButton buttonType, byte visibility)
    {
        byte buttonTypeToByte = (byte)buttonType; 

        button.gameObject.SetActive(
            (buttonTypeToByte & visibility) != 0);
    }

    
    // 메시지 박스를 초기화합니다.
    /// - titleText : 메시지 박스 타이틀에 표시될 문자열을 전달합니다.
    /// - message : 메시지 박스 내용을 전달합니다.
    /// - useButton : 표시될 버튼을 전달합니다.
    public void InitializeMessageBox(string tilteText, params MessageBoxButton[] useButton)
    {
        // 메시지 타이틀 설정
        SetTitleText(tilteText);

        // 메시지 내용 설정

        byte useButtonToByte = 0;
        foreach(MessageBoxButton use in useButton)
        {
            useButtonToByte |= (byte)use;
        }

        // 버튼 이벤트 설정
        _Button_Yes.onClick.AddListener(() => onOkButtonClicked?.Invoke(m_ScreenInstance, this));
        _Button_Cancel.onClick.AddListener(() => onCancelButtonClicked?.Invoke(m_ScreenInstance, this));

        // 버튼 표시 / 숨김
        //ButtonVisibility(_Button_Yes, MessageBoxButton.Ok, useButtonToByte);
        //ButtonVisibility(_Button_Cancel, MessageBoxButton.Cancel, useButtonToByte);
    }

    public override void CloseThisWnd()
    {
        // 메시지 박스 배경 오브젝트 제거
        if (!m_BeClose && m_MsgBoxBackground)
        {
            Destroy(m_MsgBoxBackground.gameObject);
        }
        base.CloseThisWnd();

    }
}
