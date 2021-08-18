using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuShopController : MonoBehaviour
{
    private SaveManager saveManager;

    [SerializeField] private GameObject SuperCarLockPanel;
    [SerializeField] private GameObject ZombiesLockPanel;
    [SerializeField] private GameObject HoverCarLockPanel;

    [SerializeField] private Image SuperCarBorder;
    [SerializeField] private Image ZombiesBorder;
    [SerializeField] private Image HoverCarBorder;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
        UpdatePanelInformation();
        UpdateSelectedInfo();
    }

    private void UpdateSelectedInfo()
    {
        if (saveManager.hoverCarActivated)
        {
            SelectItem(HoverCarBorder);
        }
        else if (saveManager.superCarActivated)
        {
            SelectItem(SuperCarBorder);
        }
        if (saveManager.zombiesActivated)
        {
            SelectItem(ZombiesBorder);
        }
    }

    private void UpdatePanelInformation()
    {
        if(saveManager.level1Star >= 2)
        {
            UnlockItem(SuperCarLockPanel);
            //Unlock Item
        }
        else
        {
            LockItem(SuperCarLockPanel);
            //Lock
        }
        if (saveManager.level2Star >= 2)
        {
            UnlockItem(ZombiesLockPanel);
            //Unlock Item
        }
        else
        {
            LockItem(ZombiesLockPanel);
            //Lock
        }
        if (saveManager.level3Star >= 2)
        {
            UnlockItem(HoverCarLockPanel);
            //Unlock Item
        }
        else
        {
            LockItem(HoverCarLockPanel);
            //Lock
        }
    }

    private void LockItem(GameObject gm)
    {
        gm.SetActive(true);
        gm.GetComponentInParent<Button>().interactable = false;
    }

    private void UnlockItem(GameObject gm)
    {
        gm.SetActive(false);
        gm.GetComponentInParent<Button>().interactable = true;
    }

    private void SelectItem(Image img)
    {
        ChangeColorByString(img, "#F9FF00");
    }

    private void DeselectItem(Image img)
    {
        ChangeColorByString(img, "#26CC00");
    }

    private void ChangeColorByString(Image img, string hashCode)
    {
        Color col;
        ColorUtility.TryParseHtmlString(hashCode, out col);
        col.a = 1;
        img.color = col;
        //Color changed
    }

    public void OnSuperCarBtnClick()
    {
        FindObjectOfType<MenuButtonsHandler>().PlayButtonClickSound();
        if(!saveManager.superCarActivated && saveManager.hoverCarActivated)
        {
            //Select SuperCar
            SelectItem(SuperCarBorder);
            saveManager.superCarActivated = true;

            //Desselect Hover Car
            DeselectItem(HoverCarBorder);
            saveManager.hoverCarActivated = false;
            saveManager.Save();
        }
        else if(saveManager.superCarActivated)
        {
            //Desselect
            DeselectItem(SuperCarBorder);
            saveManager.superCarActivated = false;
            saveManager.Save();
        }
        else
        {
            //Select
            SelectItem(SuperCarBorder);
            saveManager.superCarActivated = true;
            saveManager.Save();
        }
    }

    public void OnZombiesBtnClick()
    {
        FindObjectOfType<MenuButtonsHandler>().PlayButtonClickSound();
        if (saveManager.zombiesActivated)
        {
            //Desselect
            DeselectItem(ZombiesBorder);
            saveManager.zombiesActivated = false;
            saveManager.Save();

            //SkyBox
            SkyBoxUpdater skyBox = FindObjectOfType<SkyBoxUpdater>();
            skyBox.UpdateSkyBox();
        }
        else
        {
            //Select
            SelectItem(ZombiesBorder);
            saveManager.zombiesActivated = true;
            saveManager.Save();

            //SkyBox
            SkyBoxUpdater skyBox = FindObjectOfType<SkyBoxUpdater>();
            skyBox.UpdateSkyBox();
        }
    }

    public void OnHoverBtnClick()
    {
        FindObjectOfType<MenuButtonsHandler>().PlayButtonClickSound();
        if (!saveManager.hoverCarActivated && saveManager.superCarActivated)
        {
            //Select hover car
            SelectItem(HoverCarBorder);
            saveManager.hoverCarActivated = true;

            //Desselect Super Car
            DeselectItem(SuperCarBorder);
            saveManager.superCarActivated = false;
            saveManager.Save();
        }
        else if (saveManager.hoverCarActivated)
        {
            //Desselect
            DeselectItem(HoverCarBorder);
            saveManager.hoverCarActivated = false;
            saveManager.Save();
        }
        else
        {
            //Select
            SelectItem(HoverCarBorder);
            saveManager.hoverCarActivated = true;
            saveManager.Save();
        }
    }
}
