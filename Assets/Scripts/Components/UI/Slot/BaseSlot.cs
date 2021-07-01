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
    [SerializeField] private Image _Image_EquipmentSlot;

    protected ScreenInstance m_ScreenInstance;

    protected string m_InCode;

    protected SlotType m_SlotType;

    protected ItemType m_ItemType;

    protected EquipmentType m_EquipmentType;

    public Image slotImage => _Image_Slot;
    public Image equipmentSlotImage => _Image_EquipmentSlot;

    public TextMeshProUGUI countText => _TMP_Count;

    public SlotType slotType => m_SlotType;

    public ItemType itemType => m_ItemType;

    public EquipmentType equipmentType => m_EquipmentType;

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

    protected static SlotDragVisual SlotDragVisualPrefab;

    protected static Texture2D m_BgIcon;

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
        if (!m_BgIcon)
        {
            m_BgIcon = ResourceManager.Instance.LoadResource<Texture2D>(
                "Bgicon", "RPG_MMO_GUI/Texture/Props/BgIcon");
        }

        m_ScreenInstance = PlayerManager.Instance.playerController.screenInstance;

        // 빈 슬롯 기본값 설정
        SetSlotItemCount(0);
    }

    // 슬롯 초기화 메서드
    protected virtual void InitializeSlot(SlotType slotType, string inCode, EquipmentType equipmentType)
    {
        m_SlotType = slotType;
        m_InCode = inCode;
        m_EquipmentType = equipmentType;
    }


    // 슬롯에 표시되는 숫자를 설정합니다.
    public void SetSlotItemCount(int itemCount, bool visibleLessThan2 = false) =>
        _TMP_Count.text = (itemCount >= 2 || visibleLessThan2) ? itemCount.ToString() : "";

    public void ChageItem(InventorySlot inventorySlot, PlayerCharacterInfo playerCharacterInfo, QuickSlot quickSlot)
    {
        var target = playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex];

        target.itemCode = quickSlot.quickSlotInfo.itemCode;
        target.itemCount = quickSlot.quickSlotInfo.count;
        quickSlot.quickSlotInfo.count = playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex].itemCount;
        quickSlot.quickSlotInfo.itemCode = playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex].itemCode;

        playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex] = target;
    }

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
