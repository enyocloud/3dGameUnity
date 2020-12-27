@[TOC](编程实践：背包系统)

---
## 前言
本博客为第九章——UI系统的课后作业：UI 效果制作。
项目地址：[https://github.com/enyocloud/3dGameUnity/tree/master/HW9](https://github.com/enyocloud/3dGameUnity/tree/master/HW9)

---
## 项目要求
UI 效果制作（参考[第九章——UI系统](https://pmlpml.gitee.io/game-unity/post/09-ui/#6%E4%BD%9C%E4%B8%9A%E4%B8%8E%E7%BB%83%E4%B9%A0)）

 - 进入 NGUI 官方网站，使用 UGUI 实现以下效果
[Inventory 背包系统](http://www.tasharen.com/ngui/example9.html)

 - 以上例子需要使用 Unity web player， 仅支持以下操作系统与浏览器，参见官方下载。
			- Windows 版 IE11
			- Mac OS X 10.7 Safari
			- 出现界面需要等待较长时间，打开页面让它慢慢加载

---

## 项目结构
![项目结构](https://img-blog.csdnimg.cn/20201227171037256.png#pic_center)
- 主要用到了三个camera：UICamera、MainCamera、HeroCamera。
- 主要的UI界面在Sence部分实现。
用来实现UI界面鼠标交互的代码如下：
![代码](https://img-blog.csdnimg.cn/20201227172105180.png#pic_center)
---

## 实现过程
**UI层**
UI在Scene层里实现，包括背包栏和装备栏。

- UIcamera渲染整个UI界面，它挂载在Scene上的EventCamera（注意Scene的RenderMode要设置成World Space）。
![UIcamera](https://img-blog.csdnimg.cn/20201227173945148.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
- 背包栏和装备栏为Panel下新建的两个子画布，分别添加了Grid Layout Group组件。
![Grid Layout Group](https://img-blog.csdnimg.cn/2020122717435488.png#pic_center)
- 其他的图标及 Text 也在 Scene 这一层中添加完成，这里为了更好的视觉展示效果并没有添加。

**SF Scene Element**

这一层主要用来实现背景图片与背景粒子效果。

- 背景图片制成Sprite，放在名为Background的image里。
- 制作一个简单的粒子效果Particle System。
- MainCamera的组件设置如下，它的深度为最低的一个，因为它需要放在其他的后面，参数如下：
![Main Camera](https://img-blog.csdnimg.cn/20201227181126847.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
**人物**
- 人物模型是从Asset Store中下载的，挂载在新建的Hero Camera之下：
![HeroCamera](https://img-blog.csdnimg.cn/20201227181549248.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
**主要代码**
- **TiltWindow**：用来实现UI界面随鼠标转动的效果，挂在Scene层上。

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltWindow : MonoBehaviour
{
    public Vector2 range = new Vector2(5f, 3f);

    Transform mTrans;
    Quaternion mStart;
    Vector2 mRot = Vector2.zero;

    void Start ()
    {
        mTrans = transform;
        mStart = mTrans.localRotation;
    }

    void Update ()
    {
        Vector3 pos = Input.mousePosition;

        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;
        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
        float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);

        mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);
    }
}
```

- **物品拖拽**
1. **Mouse**
将如下代码挂载在新建的空对象Manager上。其中的Game manager利用实例化，可用来联系各个类，并且能够让其他类很方便地使用它里面的函数。

```csharp
using UnityEngine;
using System.Collections;
using Game_Manager;

namespace Game_Manager {

    public class Game_Scene_Manager : System.Object {
        private static Game_Scene_Manager _instance;
        private static Mouse_Image _Mouse;
        private int IsHair = 0;
        private int IsWeapon = 0;
        private int IsFoot = 0;

        public static Game_Scene_Manager GetInstance() {
            if (_instance == null) {
                _instance = new Game_Scene_Manager();
            }
            return _instance;
        }

        public void SetMouse(Mouse_Image _mouse) {
            if (_Mouse == null) {
                _Mouse = _mouse;
            }
        }

        public Mouse_Image GetMouse() {
            return _Mouse;
        }

        public void GenAll() {
            IsFoot = 1;
            IsHair = 1;
            IsWeapon = 1;
        }

        public int GetHair() { return IsHair; }
        public int GetWeapon() { return IsWeapon; }
        public int GetFoot() { return IsFoot; }

        public void SetHair(int a) { IsHair = a; }
        public void SetWeapon(int a) { IsWeapon = a; }
        public void SetFoot(int a) { IsFoot = a; }
    }

}

public class Mouse : MonoBehaviour {
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}
```

2. **Mouse_Image**
在UI层创建一个UI Image对象，挂载如下代码，实现点击图片时，图片跟随鼠标移动的效果的。实现原理是当鼠标点击一个背包栏，进行判定能够将图片移动后，图片会放入Mouse_Image中，Mouse_Image跟随着鼠标移动。

```csharp
using UnityEngine;
using System.Collections;
using Game_Manager;
using UnityEngine.UI;

public class Mouse_Image : MonoBehaviour {

    private Game_Scene_Manager gsm;
    private Image mouse_image;
    private int mouse_type = 0;
    public Sprite none;
    public Sprite hair; // 保存相应的装备
    public Sprite weapon; // 保存相应的装备
    public Sprite foot; // 保存相应的装备
    public Color None;
    public Color NotNone;
    public Camera cam;

    void Awake() {
        gsm = Game_Scene_Manager.GetInstance();
        gsm.SetMouse(this);
        mouse_image = GetComponent<Image>();
    }

    public int GetMouseType() {
        return mouse_type;
    }

    public void SetMouseType(int Mouse_type) {
        mouse_type = Mouse_type;
    }

    void Update () {
        if (mouse_type == 0) // 每一帧进行更新，检查鼠标上是否需要有装备，根据mousetype更新，如果mousetype为0则装备无
        {
            mouse_image.sprite = none;
            mouse_image.color = None;
        }
        else
        { // mousetype不为零，根据mousetype加上相应装备
            //Debug.Log("I am mouse image");
            //Debug.Log(mouse_type);

            mouse_image.color = new Color(1F, 1F, 1F, 1F);
            //Debug.Log(mouse_image.color);
            if (mouse_type == 1) mouse_image.sprite = hair;
            else if (mouse_type == 2) mouse_image.sprite = weapon;
            else if (mouse_type == 3) mouse_image.sprite = foot;
        }
        transform.position = new Vector3 (Input.mousePosition.x-600, Input.mousePosition.y-230, 0);
    }
}
```
这里需要将Image对象的初始颜色设为透明，以及其他参数调整如下：
![MouseImage](https://img-blog.csdnimg.cn/20201227185247630.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)


3. **equip**
将如下代码挂载在Equipment的没有个bottom下。

```csharp
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

```

其主要作用是使bottom里的sprite转移到鼠标上，或者使鼠标上的Sprite转移到空的装备栏中。由于三部分的是特定的装备，所以每个bottom的参数都要进行如下调整，且根据位置public变量是有所不同的。
![equip](https://img-blog.csdnimg.cn/20201227184508197.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
注意Weapon一定要调为白色，不然图片会无法显示。

4.  **MyBag**：
将如下代码挂载到背包栏的每个button上。

```csharp
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game_Manager;

public class MyBag : MonoBehaviour {
    private Game_Scene_Manager gsm;
    private Image bag_image;
    public int mouse_type = 0; // 0->没有装备，1...有不同装备
    public Sprite hair;
    public Sprite weapon;
    public Sprite foot;
    public Sprite UISprite;
    public Color weapon_color;
    public Color UISprite_color;

    void Awake()
    {
        gsm = Game_Scene_Manager.GetInstance();
        bag_image = GetComponent<Image>();
    }

    public void On_equip_Button()
    {
        Debug.Log("my bag click!");
        int MouseType = gsm.GetMouse().GetMouseType(); // 得到鼠标目前的mousetype
        if (bag_image.sprite != UISprite && MouseType == 0) // 若鼠标没有图片在上面，并且bag的image不为空有装备，则取走bag_image的装备
        {
            Debug.Log(mouse_type);
            bag_image.sprite = UISprite;
            bag_image.color = UISprite_color;
            gsm.GetMouse().SetMouseType(mouse_type); // 将当前装备的type给鼠标
            mouse_type = 0; // 此背包的mousetype变为0，则当前背包啥都没有
        }
        else
        {   // 若鼠标上有装备，则改背包的image sprite改变，根据type变为不同装备图片
            Debug.Log("my bag equipped!");
            if (MouseType == 1) bag_image.sprite = hair;
            else if (MouseType == 2) bag_image.sprite = weapon;
            else if (MouseType == 3) bag_image.sprite = foot;
            mouse_type = MouseType; // mousetype变为鼠标的mousetype
            bag_image.color = weapon_color; // 有装备了
            gsm.GetMouse().SetMouseType(0); // 鼠标装备消失
        }
    }
}
```
这里同样要修改相应的public变量：
![Mybag](https://img-blog.csdnimg.cn/2020122718502244.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

---
## 效果展示
![效果展示](https://img-blog.csdnimg.cn/20201227190749661.gif#pic_center)


---
## 参考博客
用UGUI简单实现Inventory案例：[https://blog.csdn.net/qq_20496459/article/details/51421360](https://blog.csdn.net/qq_20496459/article/details/51421360)

