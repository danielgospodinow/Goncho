using UnityEngine;

public class FloatingText : MonoBehaviour 
{
	private static readonly GUISkin skin = Resources.Load<GUISkin>("GameSkin");

	void Awake()
	{
		if(Application.loadedLevelName == "MainMenu" && Screen.height >= 1080 && skin.customStyles[0].fontSize == 30)
		{
			for(int i =0; i< skin.customStyles.Length; i++)
			{
				skin.customStyles [i].fontSize = 50;
			}
		}
	}

	public static FloatingText Show(string text, string style, IFloatingTextPositioner positioner)
	{
		GameObject go = new GameObject ("Floating Text");
		FloatingText floatingText = go.AddComponent<FloatingText> ();
		floatingText.Style = skin.GetStyle (style);
		floatingText._positioner = positioner;
		floatingText._content = new GUIContent (text);
		return floatingText;
	}

	private GUIContent _content;
	private IFloatingTextPositioner _positioner;

	public string Text 
	{
		get{ return this._content.text;}
		set{ this._content.text = value;}
	}

	public GUIStyle Style{get; set;}

	public void OnGUI()
	{
		Vector2 position = new Vector2 ();
		var contentSize = Style.CalcSize (_content);
		if(!_positioner.GetPosition(ref position, _content, contentSize))
		{
			Destroy(this.gameObject);
			return;
		}

		GUI.Label (new Rect (position.x, position.y, contentSize.x, contentSize.y), _content, Style);
	}
}
