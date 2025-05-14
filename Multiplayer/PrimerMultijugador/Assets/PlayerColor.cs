using Fusion;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerColor : NetworkBehaviour
{
	public SpriteRenderer SpriteRenderer;

	[Networked, OnChangedRender(nameof(ColorChanged))]
	public Color NetworkedColor {
		get {
			return _color;
		}
		set {
			_color = value;
		}
	}

	private Color _color;

	void ColorChanged()
	{
		SpriteRenderer.color = NetworkedColor;
	}

	void Start()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
		NetworkedColor = Random.ColorHSV();
	}

}
