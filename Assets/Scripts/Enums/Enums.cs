




// 메시지 박스에 사용되는 버튼을 나타내기 위한 열거 타입
public enum MessageBoxButton : byte
{
	Ok		=	0b0001,    
	Cancel	=	0b0010
}

public enum SlotType
{
	InventoryItemSlot,
	EquipItemSlot,
	ShopItemSlot,
	QuickSlot
}

public enum ItemType
{
	// 재료 
	Material,

	// 소비
	Consumption,

	// 장비
	Equipment,

}

public enum EquipmentType
{
	// 투구
	Helmet,

	// 갑옷
	Armor,

	// 하의
	Leg,

	// 장갑
	Glove,

	// 신발
	Shoes,

	// 무기
	Weapon


}

