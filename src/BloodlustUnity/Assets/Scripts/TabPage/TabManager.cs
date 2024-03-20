using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
	public bool OpenOnStart;
	private TabPage[] PagesCache;

	public TabPage[] Pages
	{
		get
		{
			if (PagesCache == null || !Application.isPlaying)
			{
				var foundPages = new List<TabPage>();
				foreach (Transform child in transform)
				{
					var childPage = child.GetComponent<TabPage>();
					if (childPage == null)
					{
						continue;
					}

					foundPages.Add(childPage);
				}
				PagesCache = foundPages.ToArray();
			}
			return PagesCache;
		}
	}

	private void Awake()
	{
		foreach (var page in Pages)
		{
			page.Manager = this;
		}
	}

	public void OnStart()
	{
		if (OpenOnStart)
		{
			GotoDefault();
		}
	}

	public void Goto(TabPage page)
	{
		foreach (var sibling in Pages)
		{
			sibling.TransitionOut();
		}
		page.TransitionIn();
	}

	public void Goto(int page)
	{
		foreach (var sibling in Pages)
		{
			sibling.TransitionOut();
		}
		Pages[page].TransitionIn();
	}

	public void Goto(string page)
	{
		foreach (var sibling in Pages)
		{
			sibling.TransitionOut();
		}
		foreach (var sibling in Pages)
		{
			if (sibling.name == page)
			{
				sibling.TransitionIn();
				return;
			}
		}
		throw new KeyNotFoundException($"No page with the name '{page}' is associated with the '{name}' TabManager.");
	}

	public void GotoDefault()
	{
		foreach (var page in Pages)
		{
			if (page.Default)
			{
				Goto(page);
				return;
			}
		}
	}
}
