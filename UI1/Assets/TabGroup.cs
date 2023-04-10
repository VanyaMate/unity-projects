using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<TabButton> _tabButtons = new List<TabButton>();
    [SerializeField] private List<GameObject> _tabContents = new List<GameObject>();
    [SerializeField] private Color _tabIdleColor;
    [SerializeField] private Color _tabHoverColor;
    [SerializeField] private Color _tabActiveColor;

    private TabButton _activeTab;

    public void Subscribe(TabButton tabButton)
    {
        this._tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabButton tabButton)
    {
        this.ResetTabs();
        if (tabButton != this._activeTab)
        {
            tabButton.Background.color = this._tabHoverColor;
        }
    }
    
    public void OnTabExit(TabButton tabButton)
    {
        this.ResetTabs();
    }
    
    public void OnTabSelected(TabButton tabButton)
    {
        this._activeTab = tabButton;
        this.ResetTabs(true);
        tabButton.Background.color = this._tabActiveColor;
        
        int tabIndex = tabButton.transform.GetSiblingIndex();
        for (int i = 0; i < this._tabContents.Count; i++)
        {
            if (i == tabIndex)
            {
                this._tabContents[i].SetActive(true);
            }
            else
            {
                this._tabContents[i].SetActive(false);
            }
        }
    }

    private void ResetTabs(bool hard = false)
    {
        foreach (TabButton tabButton in this._tabButtons)
        {
            if (tabButton == this._activeTab && this._activeTab != null) { continue; } 
            tabButton.Background.color = this._tabIdleColor;
        }
    }
}
