using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClosableWnd : ClosableWndBase
{
	[SerializeField] private TextMeshProUGUI Text_TitleBar;
	[SerializeField] private Button _Button_CloseWnd; 
	private ClosableWndTitlebar _ClosableWndTitlebar;

	public ClosableWndTitlebar closableWndTitlebar => _ClosableWndTitlebar;

	protected virtual void Awake()
	{
		_ClosableWndTitlebar = transform.GetComponentInChildren<ClosableWndTitlebar>();

		// 닫기 버튼이 눌린 경우 이 창을 닫도록 합니다.
		if (_ClosableWndTitlebar.closeButton != null)
		_ClosableWndTitlebar.closeButton.onClick.AddListener(CloseThisWnd);
	}

	public void SetTitleText(string titleText) => _ClosableWndTitlebar.SetTitleText(titleText);


}
