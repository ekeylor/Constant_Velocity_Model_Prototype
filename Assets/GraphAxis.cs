using UnityEngine;
using System.Collections;

public abstract class GraphAxis {

	protected abstract void Initialize();
	protected abstract void NameParts(string name);

	protected void Init(ref GameObject axisPart) {
		axisPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
		axisPart.renderer.material.color = Color.black;
	}

	public void SetLabel(string label, string text, Vector3 position) {
		GraphAxisLabel script = (GraphAxisLabel)GameObject.Find(label).GetComponent("GraphAxisLabel");
		script.SetLabel(text);
		script.SetPosition(position);
	}
}
