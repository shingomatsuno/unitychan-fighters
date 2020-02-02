using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {

	public enum PlayerType{
		Player1,
		Player2
	}

	public static int KOHAKU = 0;
	public static int KOHAKU1 = 1;
	public static int KOHAKU2 = 2;
	public static int KOHAKU3 = 3;
	public static int KOHAKU4 = 4;
	public static int KOHAKU5 = 5;
	public static int YUKO = 6;
	public static int YUKO1 = 7;
	public static int YUKO2 = 8;
	public static int YUKO3 = 9;
	public static int YUKO4 = 10;
	public static int YUKO5 = 11;
	public static int MISAKI = 12;
	public static int MISAKI1 = 13;
	public static int MISAKI2 = 14;
	public static int MISAKI3 = 15;
	public static int MISAKI4 = 16;
	public static int MISAKI5 = 17;

	public static int BLACKKOHAKU = 18;

	public enum HumanType{
		Human,
		Com
	}

	public enum Action{
		None,
		Punch,
		Kick,
		Solt,
		Special,
		Guard,
		SS
	}

	public PlayerType playerType;
	public int charaType;
	public int life = Const.MAX_LIFE;
	public int sGage = 0;
	public HumanType humanType;
}
