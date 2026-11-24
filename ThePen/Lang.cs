using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace ThePen
{
	/// <summary>
	/// Korean user interface. The XAML files keep the English text; when the Windows
	/// display language is Korean, Apply() replaces every text that has an entry here.
	/// The key is the English text without the spaces around it.
	/// </summary>
	internal static class Lang
	{
		[DllImport("kernel32.dll")]
		private static extern ushort GetUserDefaultUILanguage();

		private const int LANG_KOREAN = 0x12;

		public static readonly bool IsKorean = DetectKorean();

		private static bool DetectKorean()
		{
			try
			{
				// the primary language is the low 10 bits of the language id
				return (GetUserDefaultUILanguage() & 0x3ff) == LANG_KOREAN;
			}
			catch
			{
				return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ko";
			}
		}

		private static readonly Dictionary<string, string> korean = new Dictionary<string, string>
		{
			// main window
			["Drag to move, click to open the option menu."] = "끌면 이동, 누르면 옵션 메뉴가 열립니다.",
			["Normal mode"] = "일반 모드",
			["Draw"] = "필기",
			["Erase"] = "지우개",
			["Clear all"] = "모두 지우기",
			["Overlay"] = "오버레이",
			["Shapes"] = "도형",
			["Stamps"] = "도장",
			["Colors"] = "색",
			["Settings"] = "설정",
			["About"] = "정보",
			["Exit"] = "끝내기",

			// setting window
			["Setting"] = "설정",
			["Pens"] = "펜",
			["Pen1"] = "펜1",
			["Pen2"] = "펜2",
			["Pen3"] = "펜3",
			["Eraser Size :"] = "지우개 크기 :",
			["Shortcuts"] = "단축키",
			["Normal"] = "일반 모드",
			["Clear&Normal"] = "지우고 일반",
			["Clear"] = "모두 지우기",
			["Undo"] = "실행 취소",
			["Board1"] = "보드1",
			["Board2"] = "보드2",
			["Board3"] = "보드3",
			["Color1"] = "색1",
			["Color2"] = "색2",
			["Color3"] = "색3",
			["Color4"] = "색4",
			["Color5"] = "색5",
			["Color6"] = "색6",
			["Swap Palette"] = "팔레트 교체",
			["Stamp X"] = "도장 X",
			["Stamp ○"] = "도장 ○",
			["Stamp △"] = "도장 △",
			["Stamp □"] = "도장 □",
			["Guided Line"] = "직선",
			["Free Line"] = "자유 직선",
			["Ellipse"] = "원",
			["Rectangle"] = "사각형",
			["Grid"] = "격자",
			["Arrow"] = "화살표",
			["Overlay1"] = "오버레이1",
			["Overlay2"] = "오버레이2",
			["Overlay3"] = "오버레이3",
			["Overlay4"] = "오버레이4",
			["Global Shortcuts"] = "전역 단축키",
			["Drawing Shortcuts"] = "필기 중 단축키",
			["Always Draw Arrow"] = "항상 화살표 그리기",
			["One Key Shortcut mode :"] = "한 키 단축키 방식 :",
			["Change immediately    or"] = "바로 바꾸기    또는",
			["Change only while pressed"] = "누르고 있는 동안만 바꾸기",
			["Easy Switch and Gesture"] = "쉬운 전환과 제스처",
			["Easy Switch"] = "쉬운 전환",
			["Just hold a stylus pen to draw, move a mouse to switch back."] = "스타일러스 펜을 들면 필기 모드가 되고, 마우스를 움직이면 돌아갑니다.",
			["Edge Width For Switch :"] = "전환용 가장자리 폭 :",
			["Shake To Clear All"] = "흔들어서 모두 지우기",
			["Shake a mouse or a stylus pen about 3~4 times rapidly to clear all drawing."] = "마우스나 스타일러스 펜을 빠르게 3~4번 흔들면 필기를 모두 지웁니다.",
			["Block Touch Event"] = "터치 입력 막기",
			["Enable"] = "사용",
			["Drawing Display"] = "필기할 모니터",
			["Display1"] = "모니터1",
			["Display2"] = "모니터2",
			["Display3"] = "모니터3",
			["Display4"] = "모니터4",
			["Mouse Effect"] = "마우스 효과",
			["Move"] = "이동",
			["Left Down"] = "왼쪽 버튼",
			["Right Down"] = "오른쪽 버튼",
			["Stamp and Arrow"] = "도장과 화살표",
			["Stamp Size :"] = "도장 크기 :",
			["Arrow Size :"] = "화살표 크기 :",
			["Coming soon..."] = "준비 중입니다.",
			["Cancel"] = "취소",
			["Save and Close"] = "저장하고 닫기",
			["Set As Default"] = "기본값으로",

			// pen setting
			["Color :"] = "색 :",
			["FitToCurve"] = "곡선 다듬기",
			["Highlighter"] = "형광펜",
			["Ignore Pressure"] = "필압 무시",
			["Width :"] = "너비 :",
			["Height :"] = "높이 :",
			["Angle :"] = "각도 :",

			// messages
			["Unsaved changes will be lost."] = "저장하지 않은 변경 내용은 사라집니다.",
			["Warning"] = "경고",
			["An error occurred while loading the settings from the registry."] = "레지스트리에서 설정을 읽는 중 오류가 발생했습니다.",
			["Free to use for any purpose, including at work and in paid lectures."] = "직장과 유료 강의를 포함해 어떤 목적으로든 무료로 사용할 수 있습니다.",
			["Selling it or including it in a paid product is not allowed."] = "판매하거나 유료 제품에 포함하는 것은 허용되지 않습니다.",
		};

		/// <summary>Returns the Korean text when the display language is Korean and a translation exists.</summary>
		public static string T(string text)
		{
			if (!IsKorean || string.IsNullOrEmpty(text))
			{
				return text;
			}

			string key = text.Trim();
			if (!korean.TryGetValue(key, out string translated))
			{
				return text;
			}

			// keep the spaces around the text, the layouts rely on them
			int start = text.IndexOf(key, System.StringComparison.Ordinal);
			return text.Substring(0, start) + translated + text.Substring(start + key.Length);
		}

		/// <summary>Translates every text below the element. Call it after InitializeComponent().</summary>
		public static void Apply(object element)
		{
			if (!IsKorean || element is not DependencyObject node)
			{
				return;
			}

			if (node is Window window)
			{
				window.Title = T(window.Title);
			}

			if (node is TextBlock textBlock)
			{
				textBlock.Text = T(textBlock.Text);
			}
			else if (node is ContentControl control && control.Content is string content)
			{
				control.Content = T(content);
			}

			if (node is FrameworkElement fe)
			{
				if (fe.ToolTip is string tip)
				{
					fe.ToolTip = T(tip);
				}
				else
				{
					Apply(fe.ToolTip);
				}
			}

			foreach (object child in LogicalTreeHelper.GetChildren(node))
			{
				Apply(child);
			}
		}
	}
}
