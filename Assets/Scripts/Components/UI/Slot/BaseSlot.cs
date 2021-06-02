﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseSlot :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _TMP_Count;
    [SerializeField] private Image _Image_Slot;

    protected ScreenInstance m_ScreenInstance;

    protected string m_InCode;

    protected SlotType m_SlotType;

    public Image slotImage => _Image_Slot;
    public TextMeshProUGUI countText => _TMP_Count;

    public SlotType slotType => m_SlotType;
    // 드래그 드랍 사용 여부
    protected bool m_UseDragDrop = false;

    // 슬롯 드래그 시작
    public event System.Action<DragDropOperation, SlotDragVisual> onSlotDragStarted;

    // 슬롯 드래그 취소
    public event System.Action<DragDropOperation> onSlotDragCancelled;

    // 슬롯 드래그 끝
    public event System.Action<DragDropOperation> onSlotDragFinished;

    // 슬롯 우클릭 // 장비 장착 시 필요
    public event System.Action onSlotRightClicked;

    protected static Texture2D m_T_Null;

    private static SlotDragVisual SlotDragVisualPrefab;


    protected virtual void Awake()
    {
        if (!m_T_Null)
        {
            m_T_Null = ResourceManager.Instance.LoadResource<Texture2D>(
                "T_Null", "Image/Slot/T_NULL");
        }
        if (!SlotDragVisualPrefab)
        {
            SlotDragVisualPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "SlotDragVisual", "Prefabs/UI/Slot/SlotDragVisual").GetComponent<SlotDragVisual>();
        }
        m_ScreenInstance = PlayerManager.Instance.playerController.screenInstance;

        // 빈 슬롯 기본값 설정
        SetSlotItemCount(0);
    }

    // 슬롯 초기화 메서드
    protected virtual void InitializeSlot(SlotType slotType, string inCode)
    {
        m_SlotType = slotType;
        m_InCode = inCode;
    }


    // 슬롯에 표시되는 숫자를 설정합니다.
    public void SetSlotItemCount(int itemCount, bool visibleLessThan2 = false) =>
        _TMP_Count.text = (itemCount >= 2 || visibleLessThan2) ? itemCount.ToString() : "";


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {

        if (m_UseDragDrop)
        {
            SlotDragVisual dragVisual = Instantiate(SlotDragVisualPrefab, m_ScreenInstance.rectTransform);

            m_ScreenInstance.StartDragDropOperation(new DragDropOperation(this, dragVisual.rectTransform));

            onSlotDragStarted?.Invoke(m_ScreenInstance.dragDropOperation, dragVisual);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {

        // 드래그 드랍을 사용하지 않는다면 실행하지 않습니다.
        if (!m_UseDragDrop) return;

        if (m_ScreenInstance.dragDropOperation.overlappedComponents.Count != 0)
        {
            // 드래그 끝을 알립니다.
            onSlotDragFinished?.Invoke(m_ScreenInstance.dragDropOperation);

            // 겹친 슬롯의 onSlotDragFinished 이벤트를 발생시킵니다.
            foreach (var component in m_ScreenInstance.dragDropOperation.overlappedComponents)
            {
                if (!(component is BaseSlot)) continue;
                (component as BaseSlot).onSlotDragFinished?.Invoke(m_ScreenInstance.dragDropOperation);
            }
        }
        else
        {
            // 드래그 취소를 알립니다.
            onSlotDragCancelled?.Invoke(m_ScreenInstance.dragDropOperation);
        }

        // 드래깅 작업을 끝냅니다.
        m_ScreenInstance.FinishDragDropOperation();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 슬롯 우클릭 
            onSlotRightClicked?.Invoke();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // 드래그 드랍을 사용하지 않는다면 실행하지 않습니다.
        if (!m_UseDragDrop) return;

        // 영역 겹칩
        m_ScreenInstance.dragDropOperation?.OnPointerEnter(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        // 드래그 드랍을 사용하지 않는다면 실행하지 않습니다.
        if (!m_UseDragDrop) return;

        // 영역 겹침 끝
        m_ScreenInstance.dragDropOperation?.OnPointerExit(this);
    }

}
