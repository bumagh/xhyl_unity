using System.Collections.Generic;
using UnityEngine;

public class PPTextureManage : MonoBehaviour
{
	private static GameObject m_pMainObject;

	private static PPTextureManage m_pContainer;

	private Dictionary<string, Object[]> m_pAtlasDic;

	public static PPTextureManage getInstance()
	{
		if (m_pContainer == null)
		{
			m_pContainer = m_pMainObject.GetComponent<PPTextureManage>();
		}
		return m_pContainer;
	}

	private void Awake()
	{
		initData();
	}

	private void initData()
	{
		m_pMainObject = base.gameObject;
		m_pAtlasDic = new Dictionary<string, Object[]>();
	}

	private void Start()
	{
	}

	public Sprite LoadAtlasSprite(string _spriteAtlasPath, string _spriteName)
	{
		Sprite sprite = FindSpriteFormBuffer(_spriteAtlasPath, _spriteName);
		if (sprite == null)
		{
			Object[] array = Resources.LoadAll(_spriteAtlasPath);
			m_pAtlasDic.Add(_spriteAtlasPath, array);
			sprite = SpriteFormAtlas(array, _spriteName);
		}
		return sprite;
	}

	public void DeleteAtlas(string _spriteAtlasPath)
	{
		if (m_pAtlasDic.ContainsKey(_spriteAtlasPath))
		{
			m_pAtlasDic.Remove(_spriteAtlasPath);
		}
	}

	private Sprite FindSpriteFormBuffer(string _spriteAtlasPath, string _spriteName)
	{
		if (m_pAtlasDic.ContainsKey(_spriteAtlasPath))
		{
			Object[] atlas = m_pAtlasDic[_spriteAtlasPath];
			return SpriteFormAtlas(atlas, _spriteName);
		}
		return null;
	}

	private Sprite SpriteFormAtlas(Object[] _atlas, string _spriteName)
	{
		for (int i = 0; i < _atlas.Length; i++)
		{
			if (_atlas[i].GetType() == typeof(Sprite) && _atlas[i].name == _spriteName)
			{
				return (Sprite)_atlas[i];
			}
		}
		UnityEngine.Debug.LogWarning("图片名:" + _spriteName + ";在图集中找不到");
		return null;
	}
}
