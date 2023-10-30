using UnityEngine;

public class FK3_SortingLayerSetter : MonoBehaviour
{
	[FK3_SortingLayer]
	[SerializeField]
	private string _layerName = "Default";

	[SerializeField]
	[Range(-20000f, 30000f)]
	private int _orderInLayer;

	public virtual string LayerName
	{
		get
		{
			return _layerName;
		}
		set
		{
			_layerName = value;
			_SetLayer(LayerName, OrderInLayer);
		}
	}

	public virtual int OrderInLayer
	{
		get
		{
			return _orderInLayer;
		}
		set
		{
			_orderInLayer = value;
			_SetLayer(LayerName, OrderInLayer);
		}
	}

	protected virtual void Awake()
	{
		Renderer component = base.transform.GetComponent<Renderer>();
		if (component != null)
		{
			_layerName = component.sortingLayerName;
		}
		_SetLayer(LayerName, OrderInLayer);
	}

	protected virtual void OnValidate()
	{
		_SetLayer(LayerName, OrderInLayer);
	}

	private void _SetLayer(string layerName, int orderInLayer)
	{
	}

	public void Refresh()
	{
		_SetLayer(LayerName, OrderInLayer);
	}
}
