@[TOC](离散仿真引擎基础)

---
# 简介
学习3D游戏编程与设计的第二讲《离散仿真引擎基础》。下载试用了软件Unity3D，完成制作小游戏井字棋。

本博客分为三部分：
1. 简答题
2. 编程实践：小游戏《井字棋》
3. 思考题

---

# 简答题
**1. 解释游戏对象（GameObjects） 和 资源（Assets）的区别与联系。** 


> 1.  游戏对象表示某些资源的具体实例化，出现在游戏的场景中，游戏对象一般有敌人，场景，摄像机等非实体虚拟父类，子类一般为游戏内的实体 ；

> 	2.  资源表示硬盘中的文件，存储在Unity工程的Assets文件中，资源可以被多个对象使用，也可以作为模板实例化成游戏内的实体，一般划分为材质，对象，场景，预设，声音，脚本，贴图等子文件夹中。

 - 区别：游戏对象是具有一定属性与功能的类的实体化，对应为Unity中具有对应职能与属性的组件，例如游戏中常见的玩家、怪物等；资源是预先准备好的模型、图片、音乐等，可以直接并重复使用

 - 联系：资源可以添加到游戏对象作为其一部分，而游戏对象可以保存作为一种资源以便捷地重复使用

**2. 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）**

> 从[资源网站](http://www.aigei.com/game-code/code/unity3d/)下载游戏愤怒的小鸟、赛车游戏、2048游戏、像素鸟游戏等资源后，进行总结后，这里以愤怒的小鸟为例。

游戏资源的目录组织结构一般包含：Animation、Matreias、Prefab、Scene、Scripts、Sprites。
![截图1](https://img-blog.csdnimg.cn/20200923113841879.png#pic_center)
游戏对象树的层次结构一般包含：Camera、Background、Light和一些自定义对象等。
![截图2](https://img-blog.csdnimg.cn/20200923114114469.png#pic_center)

**3. 编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件**

- 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
-  常用事件包括    OnGUI() OnDisable() OnEnable()


>编写如下代码并添加到Main Camere：
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start");
    }
    
    void Update()
    {
        Debug.Log("Update");
    }
    
    void Awake()
    {
        Debug.Log("Awake");
    }
    
    void FixedUpdate()
    {
        Debug.Log("FixedUpdate");
    }
    
    void LateUpdate()
    {
        Debug.Log("LateUpdate");
    }
    
    void OnGUI()
    {
        Debug.Log("OnGUI");
    }
    
    void OnDisable()
    {
        Debug.Log("OnDisable");
    }
    
    void OnEnable()
    {
        Debug.Log("OnEnable");
    }
}
```
> 运行结果如下：

![截图3](https://img-blog.csdnimg.cn/20200923000930709.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

> - Awake() ：当一个脚本实例被载入时被调用；
> - Start() :  在所有update()函数被调用之前调用；
>  - Update()：当行为启用时其Update()在每一帧被调用；
>  - FixedUpdate()：当行为启动时其Update()在每一时间片被调用； 
>  - LateUpdate()：当行为启用时，在所有Update()执行完后执行；
>  - Onable() ：当对象变为可用或激活状态时此函数被调用，OnEnable不能用于协同程序（物体启动时被调用）；
>  - Ondisable() ：当对象变为不可用或非激活状态时此函数被调用。当物体被销毁时他被调用，并且可用于任意清理代码 。当脚本编译完成之后重新加载时，该函数被调用，OnEnable()在脚本被载入后被调用。(物体被禁用时调用)  ；
>  - OnGUI() ：绘制GUI时触发。一般在这个函数里绘制GUI菜单(GUI显示函数只能在OnGUI中被调用）。


**4. 查找脚本手册，了解 GameObject，Transform，Component 对象**
 **- 分别翻译官方对三个对象的描述（Description）**
> GameObeject：Base class for all entities in Unity Scenes.

翻译：Unity场景中所有实体的基类。

> Transform：Position, rotation and scale of an object.Every object in a Scene has a Transform. It's used to store and manipulate the position, rotation and scale of the object. Every Transform can have a parent, which allows you to apply position, rotation and scale hierarchically.

翻译：对象的位置旋转和比例。场景中每个对象都有一个变换。他用于操作对象的位置，缩放和旋转。每个转换都可以拥有一个父类，允许分层应用位置。

> Component：Base class for everything attached to GameObjects.

翻译：所有附加到GameObject的东西的基类。

**描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件** 
 - 本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应API。
 - 例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。

![截图4](https://img-blog.csdnimg.cn/20200923003659606.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

> table：第一个选择框时便签Tag：当前选择的是未加标签；第二个选择框是层，当前是默认属性；第三个属性是预设。  
> table的Transform：有三个属性：Position、Rotation、Scale，分别表示位置，旋转，比例  
>  table的部件：Transform、Cube、Mesh Renderer、Box Colloder等。

 **用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）**

![截图5](https://img-blog.csdnimg.cn/20200923011328354.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)


**5. 资源预设（Prefabs）与 对象克隆 (clone)**
 - 预设（Prefabs）有什么好处？

> 使游戏对象可重复使用，这个游戏对象已经配置完成，可以直接加入到游戏中去，提高了工作效率。

 - 预设与对象克隆 (clone or copy or Instantiate of Unity
   Object) 关系？

> 预设与克隆都可以由一个对象生成大量相同的对象，但是预设出的对象在本体改变时回随着本体进行改变，而克隆出的对象之间没有联系，可以随意更改。

-  制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象

> 参考课件，制作table后将其拖入Assets，自动形成table预制。

![截图6](https://img-blog.csdnimg.cn/20200923115556244.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

> 将 table 预制资源实例化成游戏对象，代码如下：


```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createTable : MonoBehaviour
{
    public GameObject table;
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = (GameObject)Instantiate(table, transform.position, transform.rotation);
        temp.name = "instance";
    }
    void Update()
    {
    
    }
}

```

> 将createTable添加到Main Camera，并把之前做好的table预制拖入选择框。

![截图7](https://img-blog.csdnimg.cn/20200923120238499.png#pic_center)

> 点击运行，效果如下图：

![截图8](https://img-blog.csdnimg.cn/20200923120645961.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

---
# ==编程实践，小游戏==
**井字棋——**

- 游戏内容： 井字棋 或 贷款计算器 或 简单计算器 等等
- 技术限制： 仅允许使用 IMGUI 构建 UI
- 作业目的：
	- 了解 OnGUI() 事件，提升 debug 能力
	- 提升阅读 API 文档能力

> 实现功能：
> - 共O、X两种棋子，点击Start游戏开始；
> - O棋先手，点击方块O、X棋子交替落棋；
> - 两种赢法：横着三格连成，竖着三格连成；
> - 三种结局：O棋赢、X棋赢、平局。

主要代码如下：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private int[,] chessBoard = new int[3, 3];    
    private int turn = 1;//决定谁落子

    void Start()    
    {        
    Reset();//初始化界面
    }
    int checkWin()//判断胜负
    {
        for(int i = 1; i < 3; i++)
        {
            if (chessBoard[i, 0] == chessBoard[i, 1] && chessBoard[i, 0] == chessBoard[i, 2] && chessBoard[i, 0] != 0)
                return chessBoard[i, 0];
        }//竖着赢
        for (int i = 0; i < 3; i++)
        {
            if (chessBoard[0, i] == chessBoard[1, i] && chessBoard[0, i] == chessBoard[2, i] && chessBoard[0, i] != 0)
                return chessBoard[0, i];
        }//横着赢
        if (chessBoard[0, 0] == chessBoard[1, 1] && chessBoard[0, 0] == chessBoard[2, 2] && chessBoard[0, 0] != 0)
            return chessBoard[0, 0];//斜着赢
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (chessBoard[i, j] != 0)
                {
                    count++;
                }
            }
        }

        if (count == 9)//被填满仍无人胜利，平局
            return 0;        
        return 3;
    }
    
    void OnGUI()
    {
        if (GUI.Button(new Rect(300, 50, 150, 50), "Start Game"))        
        {           
            Reset();//点击Start Game按钮重置游戏        
        }
        GUISkin skin = GUI.skin;
        skin.button.normal.textColor = Color.green;        
        skin.button.hover.textColor = Color.blue;//设置按钮样式        
        int State = checkWin();//判断当前状态2:x赢1：O赢0：平局        
        if (State == 2)
        {
            skin.button.normal.textColor = Color.red;
            GUI.Label(new Rect(300, 105, 50, 50), "X Win!");
        }
        else if (State == 1)        
        {      
            skin.button.normal.textColor = Color.red;
            GUI.Label(new Rect(300, 105, 50, 50), "O Win");
        }        
        else if (State == 0)        
        {            
            skin.button.normal.textColor = Color.red;            
            GUI.Label(new Rect(300, 105, 50, 50), "Tied");    
        }        
        for (int i = 0; i < 3; i++)        
        {
            for (int j = 0; j < 3; j++)            
            {                
                if (chessBoard[i, j] == 1)               
                {
                    GUI.Button(new Rect(300 + 50 * i, 130 + j * 50, 50, 50), "O");
                }
                else if (chessBoard[i, j] == 2)                
                {                    
                    GUI.Button(new Rect(300 + 50 * i, 130 + j * 50, 50, 50), "X");                
                }
                if (GUI.Button(new Rect(300 + 50 * i, 130 + j * 50, 50, 50), ""))
                {//按钮没被点击时，被点击后改变棋盘状态，然后根据棋盘状态改变按钮显示
                    if (State == 3)                    
                    {                        
                        if (turn == 1)                            
                            chessBoard[i, j] = 1;                        
                        else if (turn == -1)                            
                            chessBoard[i, j] = 2;                        
                        turn = -turn;
                    }
                }
            }
        }
     }
     void Reset()    
     {//重置棋盘        
        turn = 1;        
        for (int i = 0; i < 3; i++)        
        {            
            for (int j = 0; j < 3; j++)            
            {                
                chessBoard[i, j] = 0;            
            }        
        }    
    }
    void Update()    
    {
             
    }        
}

```

> 实现效果如下：

开始游戏
![井字棋开始](https://img-blog.csdnimg.cn/20200923131614913.png#pic_center)
O棋横着赢
![井字棋横着赢](https://img-blog.csdnimg.cn/20200923131445935.png#pic_center)
X棋横着赢
![井字棋横着赢2](https://img-blog.csdnimg.cn/20200923131702448.png#pic_center)
O棋竖着赢
![井字棋竖着赢](https://img-blog.csdnimg.cn/20200923131813738.png#pic_center)
X棋竖着赢
![井字棋竖着赢2](https://img-blog.csdnimg.cn/20200923132015813.png#pic_center)
平局
![井字棋平局](https://img-blog.csdnimg.cn/20200923132047296.png#pic_center)

----

# 思考题（选做）
**- 微软 XNA 引擎的 Game 对象屏蔽了游戏循环的细节，并使用一组虚方法让继承者完成它们，我们称这种设计为“模板方法模式”。**
	- 为什么是“模板方法”模式而不是“策略模式”呢？

> 模板方法模式：定义一个算法的骨架，将骨架中的特定步骤延迟到子类中。模板方法模式使得子类可以不改变算法的结构即可重新定义该算法的某些特定步骤。
> 策略模式：属于对象的行为模式。其用意是针对一组算法，将每一个算法封装到具有共同接口的独立的类中，从而使得它们可以相互替换。
> **模板方法可以通过不同的实现来达到不同的效果，而策略模式以不同的实现方法来达到相同的效果。**

**- 将游戏对象组成树型结构，每个节点都是游戏对象（或数）。**
	- 尝试解释组合模式（Composite Pattern / 一种设计模式）。
	- 使用 BroadcastMessage() 方法，向子对象发送消息。你能写出 BroadcastMessage() 的伪代码吗?

> 组合模式（Composite Pattern），又叫部分整体模式，是用于把一组相似的对象当作一个单一的对象。组合模式依据树形结构来组合对象，用来表示部分以及整体层次。这种类型的设计模式属于结构型模式，它创建了对象组的树形结构。

```csharp
foreach childObject 
     sendMessage();
```


- **一个游戏对象用许多部件描述不同方面的特征。我们设计坦克（Tank）游戏对象不是继承于GameObject对象，而是 GameObject 添加一组行为部件（Component）。**
- 这是什么设计模式？

> Decorator模式

- 为什么不用继承设计特殊的游戏对象？

> 因为需要对对象动态的添加功能，采用继承需要修改的代码过多，采用Decorate模式，可以在不修改原来代码的情况下增加新的功能核模块。且继承容易导致类与类之间结构混乱。
---
**参考资料：**
[模板方法模式和策略模式](https://blog.csdn.net/shensky711/article/details/53418034)
[组合模式](https://www.runoob.com/design-pattern/composite-pattern.html)
[装饰器模式](https://www.runoob.com/design-pattern/decorator-pattern.html)
