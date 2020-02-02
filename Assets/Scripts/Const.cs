using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const {


	public static int MAX_LIFE = 100;
	public static int MAX_S_GAGE = 100;
	public static int DAMAGE_LITE = 3;
	public static int DAMAGE_MIDDLE = 7;
	public static int DAMAGE_BIG = 10;
	public static int DAMAGE_SPECIAL = 15;
	public static int DAMAGE_SS = 25;

	//コインカンスト値
	public static int MAX_COIN = 99999;
	public static string KEY_PLAYERPOS = "KEY_PLAYERPOS";
	public static string KEY_BGM_ON = "KEY_BGM_ON";
	public static string KEY_SE_ON = "KEY_SE_ON";
	public static string KEY_TOP_KACHINUKI = "KEY_TOP_KACHINUKI";
	public static string KEY_COIN = "KEY_COIN";
	public static string KEY_ENABLE_CHARAS = "KEY_ENABLE_CHARAS";
	public static string ENABLE_CHARAS_DEFAULT = PlayerInfo.KOHAKU.ToString() + "," + PlayerInfo.YUKO.ToString() + "," + PlayerInfo.MISAKI.ToString();
	public static string KEY_LEVEL = "KEY_LEVEL";
	public static string KEY_KATINUKI_MAX_SCORE = "KEY_KATINUKI_MAX_SCORE";

	public const string TAG_PUNCH = "Punch";
	public const string TAG_KICK = "Kick";
	public const string TAG_KICK_AFTER = "KickAfter";
	public const string TAG_SOLT = "Solt";
	public const string TAG_SPECIAL = "Special";
	public const string TAG_SPECIAL_START = "SpecialStart";
	public const string TAG_SPECIAL_UPPER = "SpecialUpper";
	public const string TAG_SPECIAL_FALL = "SpecialFall";
	public const string TAG_PLAYER = "Player";
	public const string TAG_GUARD = "Guard";
	public const string TAG_NODAMAGE = "NoDamage";
	public const string TAG_HEADSPRING = "Headspring";
	public const string TAG_NOMOVE = "NoMove";
	public const string TAG_DEAD = "Dead";
	public const string TAG_LITE_DAMAGE = "LiteDamage";
	public const string TAG_MIDDLE_DAMAGE = "MiddleDamage";
	public const string TAG_DOWN = "Down";
	public const string TAG_HADOUKEN = "Hadouken";

	public static string UNITYADS_ANDROID_ID = "XXXXXXX";
	public static string UNITYADS_IOS_ID = "XXXXXXX";

	public static string ADMOB_ANDROID_ID = "ca-app-pub-XXXXXXXXXXXX/XXXXXXXXXXXX";
	public static string ADMOB_IOS_ID = "ca-app-pub-XXXXXXXXXXXX/XXXXXXXXXXXX";

	public static string TEST_DEVICE_ID = "XXXXXXXXXXXXXXXXXXXXXXXX";

	public static string PLAYSTORE_URL = "";
	public static string APPSTORE_URL = "https://play.google.com/store/apps/details?id=com.hayabusa0909.chibicharafighters";

	public static string SHARE_TEXT_ARCADE = "アーケードモードで{0}ちゃんにかったよ♪";
	public static string SHARE_TEXT_VS = "たいせんモードであそんだよ♪"; 
	public static string SHARE_TEXT_ENDRESS = "かちぬきモードで{0:D}にんぬきしたよ♪";

	public static string GAME_NAME = "ちびキャラファイターズ";

	public static int GAME_LEVEL_EASY = 1;
	public static int GAME_LEVEL_NOMAL = 2;
	public static int GAME_LEVEL_HEARD = 3;
}
