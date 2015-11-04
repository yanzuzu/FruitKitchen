public class Config
{
	public static int ItemMaxNum = 999;

	public const int BLOCK_INIT_CREATE_NUM = 100;

	public const int DIRECT_NO_DIR = 99;
	public const int DIRECT_UP = 100;
	public const int DIRECT_DOWN = 101;
	public const int DIRECT_LEFT = 102;
	public const int DIRECT_RIGHT = 103;

	public const int MAX_PLAY_NUM = 5;
	public const int RECOVER_TIME = 30;//1800;
	
	public const int MAX_WARM_HOLE_NUM = 10;
	public const int MAX_CREATE_STAGE_NUM = 10;

	public const int NEED_COIN_TURN = 3;
	public const int NEED_COIN_FREE_MOVE = 6;
	public const int NEED_COIN_BOMB_DIRECT = 4;

	public const int SCORE_ERASE_BLOCK = 10;   
	public const int SCORE_ERASE_SCORE_BLOCK = 1000;

	public const int SPECIAL_BOMB_MAX_NUM = 2;
	public const float BOMB_SHINING_TIME = 0.1f;
	public const float BOMB_UP_EFFECT_TIME = 0.16f;

	public const float BLOCK_UPDATE_TIME = 0.03f;
	public const float ERASE_UPDATE_TIME = BLOCK_UPDATE_TIME * (float)CHECK_BLOCK_NUM;
	public const int CHECK_BLOCK_NUM = 6;
	public const float BLOCK_MOVE_TIME = 0.1f;
	public const float BLOCK_SWITCH_TIME = 0.15f;

	public const int CHALLENG_MAX_NUM = 5;
	public const int CHALLENG_LIMMIT_TIME = 24 * 60 * 60;

	public const int UNLOCK_BIG_STAGE_STAR_NUM = 45;
	public const int UNLOCK_GOLD_ITEM_STAR_NUM = 60;
	public const int MAX_BIG_STAGE_NUM = 5;

	public static int WIDTH_NUM = 9;
	public static int HEIGHT_NUM = 11;
	public static int SLOT_SIZE_WIDTH = 35;
	public static int SLOT_SIZE_HEIGHT = 35;
	public static int BLOCK_SIZE_WIDTH = 30;
	public static int BLOCK_SIZE_HEIGHT = 30;
	public static int WIDTH_WITH_EDGE = 18;
	public static int HEIGHT_WITH_EDGE = 110;
	public const int GOLD_BLOCK = 99;
	public const int WARM_HOLE_START = 100;
	public const int WARM_HOLE_END = 119;
	public const int WOOD_WARM_HOLE_START = 120;
	public const int WOOD_WARM_HOLE_END = 139;
	public const int GLASS_WARM_HOLE_START = 140;
	public const int GLASS_WARM_HOLE_END = 159;
	public const int H_WOOD_WARM_HOLE_START = 160;
	public const int H_WOOD_WARM_HOLE_END = 179;
	public const int FIELD_ITEM_START = 200;
	public const int FIELD_ITEM_END = 299;
	public const int CARD_ITEM_BOMB_CONNECT = 201;
	public const int CARD_ITEM_BOMB_HORIZONTAL = 202;
	public const int CARD_ITEM_BOMB_VERTICAL = 203;
	public const int CARD_ITEM_BOMB_DOUBLE = 204;
	public const int CARD_ITEM_BOMB_UP = 205;
	public const int CARD_ITEM_KNIFE_ERASE = 206;
	public const int CARD_ITEM_WARM_HOLE = 207;
	public const int GOLD_ITEM_LIGHTING = 251;
	public const int GOLD_ITEM_SHOVEL = 252;
	public const int GOLD_ITEM_BOMBER = 253;
	public const int GOLD_ITEM_ATOMIC = 254;
	public const int COUNT_DOWN_BOMB_START = 301;
	public const int COUNT_DOWN_BOMB_END = 399;

	public const string MESSAGE_SOURCE_FROM_SYSTEM = "-9999";
	public const float SYNCHRONOUS_GAME_RECORD_TIME = 60.0f;
	public const float UPDATE_GAME_RECORD_INFO_IN_UI = 30.0f;

	public const int CHALLENG_SCORE_TYPE_MOVE = 1;
	public const int CHALLENG_SCORE_TYPE_BOMB = 2;

	public const int DAILY_GIFT_RESET_TIME = 24 * 60 * 60;

	public const int PLAY_RESULT_LAYER_SUCCESS = 100;
	public const int PLAY_RESULT_LAYER_FAIL = 101;

	public const int CHALLENG_WIN_REWARD_ITEM_ID = 8;
	public const int CHALLENG_WIN_REWARD_ITEM_NUM = 2;

	public const int DAILY_GIFT_ITEM_ID = 5;
	public const int DAILY_GIFT_ITEM_NUM = 2;

	public const int MESSAGE_INVITE_UNLOCK_BIG_STAGE = 100;
	public const int MESSAGE_INVITE_UNLOCK_GOLD_ITEM = 101;

	public const int MESSAGE_TYPE_CHALLENG_FROM_OTHER = 1;
	public const int MESSAGE_TYPE_CHALLENG_GIFT = 2;        
	public const int MESSAGE_TYPE_GIFT = 3;      
	public const int MESSAGE_INVITE_UNLOCK = 4; 

	public const int PLAYER_DEFAULT_OWN_COINS = 300;       
	public const int PLAYER_DEFAULT_OWN_ITEM_NUM = 10;

	public const int BORN_FIELD_ITEM_SLOT_MAX_PER_TIME = 5;
	
	public const int COIN_NUM_PER_UNIT = 15;
	public const int MOVE_NUM_PER_UNIT = 5;
	
	public const int TEACH_MODE_ALLOW_ACIOTN_TOUCH = 100;
	public const int TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH = 101;
	public const int TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP = 102;
	public const int TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN = 103;
	public const int TEACH_MODE_ALLOW_ACIOTN_SWIPE_LEFT = 104;
	public const int TEACH_MODE_ALLOW_ACIOTN_SWIPE_RIGHT = 105;
	public const int TEACH_MODE_ALLOW_ACIOTN_PRESS_FREE_MOVE_BTN = 109;
	public const int TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_DIRECT_BTN = 110;
	public const int TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_HORIZONTAL_BTN = 111;

	public const int TEACH_MODE_FINGER_ARROW = 200;
	public const int TEACH_MODE_FINGER_TOUCH = 201;
	public const int TEACH_MODE_FINGER_SWIPE = 202;

	public enum Slot_Enum
	{
		Slot_Block                  =     0,     
		Slot_Normal,                             
		Slot_Normal_NO_Block,                    
		Slot_Born_Block_All,                     
		Slot_Born_Block_Product,                 
		Slot_Born_Block_Normal,                  
		Slot_Score,                              
		Slot_WarmHole_Start,                     
		Slot_WarmHole_End,                       
		Slot_Normal_With_CountDown_Bomb,         
		Slot_Normal_With_ConnectOnly_Bomb,       
		Slot_Normal_With_WoodBlock,              
		Slot_Normal_With_GrassBlock,             
		Slot_Normal_With_SauceBlock,             
		Slot_Normal_With_MilkBlock,              
		Slot_Normal_With_FieldItem,              
		Slot_Born_Field_Item,                    
		Slot_Born_Product_Block,                 
		Slot_Normal_With_WoodBlock_Break,        
	}

	public enum Block_Enum
	{
		Block_Normal                =     0,
		Block_Score,                        
		Block_Produce,                      
		Block_Small_Bomb,                   
		Block_Big_Bomb,                     
		Block_Bomb_CountDown,               
		Block_Bomb_ConnectOnly,             
		Block_Wood,                         
		Block_Grass,                        
		Block_Sauce,                        
		Block_Milk,                         
		Block_Field_Item,                   
		Block_Bomb_Fruit,
	}

	public enum Born_Slot_Enum
	{
		Born_Slot_Null				=       0,
		Born_Slot_All,                            
		Born_Slot_ProductOnly,                    
		Born_Slot_NormalOnly,
	}

	public enum Block_Color_Enum
	{
		Block_Color_Null            =       -1,
		Block_Color_1               =       0, 
		Block_Color_2,                         
		Block_Color_3,                         
		Block_Color_4,                         
		Block_Color_5,                         
		Block_Color_6,                         
		Block_Color_Num,                       
		Block_Product_1,                       
		Block_Product_2,                       
		Block_Product_3,                       
		Block_Product_4,                       
		Block_Product_5,                       
		Block_Product_6,                              
	}

	public enum Operate_Mode_Enum
	{
		Operate_Mode_Null               =       0,
		Operate_Mode_Around_Move,                 
		Operate_Mode_Free_Move,                   
		Operate_Mode_Bomb_Direct,                 
		Operate_Mode_Wormhole,                    
		Operate_Mode_Bomb_Upgrade,                
		Operate_Mode_Bomb_Horizontal,             
		Operate_Mode_Bomb_Vertical,               
		Operate_Mode_Bomb_Twice,                  
		Operate_Mode_Erase_Knife,                 
		Operate_Mode_Bomb_Connect,                
		Operate_Mode_Lignting,                    
		Operate_Mode_Shovel,                      
		Operate_Mode_Bomber,                      
		Operate_Mode_Atomic,                      
		Operate_Mode_Max,                            
	}

	public enum Player_Info_Enum
	{
		Player_Info_Score               = 1,
		Player_Info_Free_Fall_Num,          
		Player_Info_Bombed_Block_Num,       
		Player_Info_Gold_Coin,              
		Player_Info_Erase_Fruit_1_Num,      
		Player_Info_Erase_Fruit_2_Num,      
		Player_Info_Erase_Fruit_3_Num,      
		Player_Info_Erase_Fruit_4_Num,      
		Player_Info_Erase_Fruit_5_Num,      
		Player_Info_Erase_Fruit_6_Num,      
		Player_Info_Use_Fruit_Bomb_Num,     
		Player_Info_Use_Small_Bomb_Num,     
		Player_Info_Use_Big_Bomb_Num,       
		Player_Info_Left_Move_Num,          
		Player_Info_Fruit_1_Num,            
		Player_Info_Fruit_2_Num,            
		Player_Info_Fruit_3_Num,            
		Player_Info_Fruit_4_Num,            
		Player_Info_Fruit_5_Num,            
		Player_Info_Fruit_6_Num,                                        
	}

	public enum Refill_Block_Step_Enum
	{
		Refill_Block_Null                =          0,
		Refill_Block_Player_Press_Reset,              
		Refill_Block_PrepareAddNewBlock,                                                     
	}

	public enum Item_Effect_Enum
	{
		Item_Effect_Coin                =       1, 
		Item_Effect_Love_Heart          =       2, 
		Item_Effect_Move                =       3, 
		Item_Effect_Bomb_Up             =       4, 
		Item_Effect_Knife_Erase         =       5, 
		Item_Effect_Warm_Hole           =       6, 
		Item_Effect_Bomb_Connect        =       7, 
		Item_Effect_Bomb_Twice          =       8, 
		Item_Effect_Bomb_Lignting       =       9, 
		Item_Effect_Bomb_Shovel         =       10,
		Item_Effect_Bomb_Bomber         =       11,
		Item_Effect_Bomb_Atomic         =       12,
		Item_Effect_Unlock_Stage        =       13,                                                   
	}

	public enum Pass_Stage_Record_Enum
	{
		Pass_Stage_Record_Null			=       0,     
		Pass_Stage_Record_Star_1,                       
		Pass_Stage_Record_Star_2,         
		Pass_Stage_Record_Star_3,                                          
	}

	public enum Pass_Stage_Enum
	{
		Pass_Stage_Star_1				=       1,
		Pass_Stage_Star_2,
		Pass_Stage_Star_3,                                            
	}

	public enum Teach_Mode_Id_Meaning_Enum
	{
		Teach_Mode_Space                                    =       0,          
		Teach_Mode_Fruit_Banana                             =       1,          
		Teach_Mode_Fruit_Strawberries                       =       2,          
		Teach_Mode_Fruit_Kiwi                               =       3,          
		Teach_Mode_Fruit_Grapes                             =       4,          
		Teach_Mode_Fruit_Mango                              =       5,          
		Teach_Mode_Fruit_Muskmelon                          =       6,          
		Teach_Mode_Fruit_Knife                              =       7,          
		Teach_Mode_Fruit_Banana_With_Gold                   =       101,        
		Teach_Mode_Fruit_Strawberries_With_Gold             =       102,        
		Teach_Mode_Fruit_Kiwi_With_Gold                     =       103,        
		Teach_Mode_Fruit_Grapes_With_Gold                   =       104,        
		Teach_Mode_Fruit_Mango_With_Gold                    =       105,        
		Teach_Mode_Fruit_Muskmelon_With_Gold                =       106,        
		Teach_Mode_Born                                     =       201,        
		Teach_Mode_Receive                                  =       202,                                         
	}

	public enum Message_Type_Enum
	{
		Message_Type_Unlock_Stage				=       1,          
                                        
	}
}

