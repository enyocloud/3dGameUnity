using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game_Manager;

public class equip : MonoBehaviour {

    private Game_Scene_Manager gsm;
    private Image equip_image;
    public int mouse_type;
    public Sprite weapon;
    public Sprite UISprite;
    public Color weapon_color;
    public Color UISprite_color;

    void Awake()
    {
        gsm = Game_Scene_Manager.GetInstance();
        equip_image = GetComponent<Image>();
    }

    public void On_equip_Button() {
        int MouseType = gsm.GetMouse().GetMouseType();
        if (equip_image.sprite == weapon && (MouseType == 0 || MouseType == mouse_type))
        {
            equip_image.sprite = UISprite;
            equip_image.color = UISprite_color;
            gsm.GetMouse().SetMouseType(mouse_type);
        }
        else
        {
            if (MouseType == mouse_type) {
                equip_image.sprite = weapon;
                equip_image.color = weapon_color;
                gsm.GetMouse().SetMouseType(0);
            }
        }
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (mouse_type == 1 && gsm.GetHair() == 1)
        {
            gsm.SetHair(0);
            equip_image.sprite = weapon;
            equip_image.color = weapon_color;
        } else if (mouse_type == 2 && gsm.GetWeapon() == 1)
        {
            gsm.SetWeapon(0);
            equip_image.sprite = weapon;
            equip_image.color = weapon_color;
        } else if (mouse_type == 3 && gsm.GetFoot() == 1)
        {
            gsm.SetFoot(0);
            equip_image.sprite = weapon;
            equip_image.color = weapon_color;
        }
    }
}
