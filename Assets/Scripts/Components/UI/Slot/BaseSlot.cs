using System.Collections;
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

    protected Button m_Button_Slot;

    protected string m_InCode;

    protected SlotType m_SlotType;

    public Image slotImage => _Image_Slot;

    public TextMeshProUGUI countText => _TMP_Count;

    // 드래그 드랍 사용 여부
    protected bool m_UseDragDrop = false;

    // 슬롯 드래그 시작
    public event System.Action<DragDropOperation, SlotDragVisual> onDragStarted;

    // 슬롯 드래그 취소
    public event System.Action<DragDropOperation> onDragCancelled;

    // 슬롯 드래그 끝
    public event System.Action<DragDropOperation> onDragFinished;

    // 슬롯 우클릭 // 장비 장착 시 필요
    public event System.Action onSlotRightClicked;

    protected static Texture2D m_T_NULL;

    private static SlotDragVisual SlotDragVisualPrefab;


    protected virtual void Awake()
    {
        if (!m_T_NULL)
        {
            m_T_NULL = ResourceManager.Instance.LoadResource<Texture2D>(
                "f", "Image/ItemImage/f");
        }
        if (!SlotDragVisualPrefab)
        {
            SlotDragVisualPrefab = ResourceManager.Instance.LoadResource<SlotDragVisual>(
                "SlotDragVisual", "Prefabs/UI/Slot/SlotDragVisual");
        }
        m_ScreenInstance = PlayerManager.Instance.playerController.screenInstance;

        m_Button_Slot = GetComponent<Button>();

    }

    // 슬롯 초기화 메서드
    protected virtual void InitializeSlot(SlotType slotType, string inCode)
    {
        m_SlotType = slotType;
        m_InCode = inCode;
    }


    // 슬롯 아이템의 카운트의 개수가 0이면 표시하지 않습니다.
    public void SetItemCount()
    {
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (m_UseDragDrop)
        {
            SlotDragVisual dragVisual = Instantiate(SlotDragVisualPrefab, m_ScreenInstance.rectTransform);

            m_ScreenInstance.StartDragDropOperation(new DragDropOperation(this, dragVisual.rectTransform));

            onDragStarted?.Invoke(m_ScreenInstance.dragDropOperation, dragVisual);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
    }
}
