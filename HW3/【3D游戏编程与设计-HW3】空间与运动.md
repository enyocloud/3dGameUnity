@[TOC](空间与运动)

---
# 简答题
**· 游戏对象运动的本质是什么？**

> 运动的本质是游戏对象通过脚本变化其（position）位置，（rotation）欧拉角，（scale）形状。

**· 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）**

> 1. 改变Transform属性：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class move1 : MonoBehaviour
{
	public float Power = 10;//这个代表发射时的速度/力度等，可以通过此来模拟不同的力大小
 	public float Angle = 45;//发射的角度，这个就不用解释了吧
 	public float Gravity = -10;//这个代表重力加速度
 	
 	private Vector3 MoveSpeed;//初速度向量
 	private Vector3 GritySpeed = Vector3.zero;//重力的速度向量，t时为0
 	private float dTime;//已经过去的时间
 	private Vector3 currentAngle;
	
	void Start()
 	{
     		//通过一个公式计算出初速度向量
     		//角度*力度
     		MoveSpeed = Quaternion.Euler(new Vector3(0, 0, Angle)) * Vector3.right * Power;
     		currentAngle = Vector3.zero;
 	}

	void FixedUpdate()
	{
     		//计算物体的重力速度
     		//v = at ;
     		GritySpeed.y = Gravity * (dTime += Time.fixedDeltaTime);
     		//位移模拟轨迹
     		transform.position += (MoveSpeed + GritySpeed) * Time.fixedDeltaTime;
     		currentAngle.z = Mathf.Atan((MoveSpeed.y + GritySpeed.y) / MoveSpeed.x) * Mathf.Rad2Deg;
     		transform.eulerAngles = currentAngle;
 	}
}
```

> 2. 使用Vector3的方法：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2 : MonoBehaviour
{
	public float dTime = 0f;//已经过去的时间
    	public float speedx0 = 5.0f;
    	public float speedy0 = 1.0f;
    	public float ay = 1.0f;

	void Start()
	{
	
	}

	void Update()
    	{
        	float time = dTime + Time.deltaTime;
        	float dx = speedx0 * Time.deltaTime;
        	float dy = (float)(speedy0 * Time.deltaTime + 0.5 * ay * (time * time - dTime * dTime));
        	this.transform.position += Vector3.right * dx;
        	this.transform.position += Vector3.down * dy;
        	dTime = time;
    	}
}

```

> 利用transform中的translate函数来改变position：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3 : MonoBehaviour
{
    	public float speed = 1;
    	void Start()
    	{
    
    	}
    	void Update()
    	{
        	Vector3 move = new Vector3(Time.deltaTime * 5, -Time.deltaTime * (speed / 10), 0);
        	this.transform.Translate(move);
        	speed++;
    	}
}
```

**· 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。**

具体代码如下：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class solarsys : MonoBehaviour
{
    	public Transform mercury;
    	public Transform venus;
    	public Transform earth;
    	public Transform mars;
    	public Transform jupiter;
    	public Transform saturn;
    	public Transform uranus;
    	public Transform neptune;
    	
    	void Start () 
    	{
        	mercury.position = new Vector3 (3, 0, 0);
        	venus.position = new Vector3 (-5, 0, 0);
        	earth.position = new Vector3 (7, 0, 0);
        	mars.position = new Vector3 (-9, 0, 0);
        	jupiter.position = new Vector3 (-11, 0, 0);
        	saturn.position = new Vector3 (13, 0, 0);
        	uranus.position = new Vector3 (15, 0, 0);
        	neptune.position = new Vector3 (-17, 0, 0);
    	}
    	
    	void Update () 
    	{
        	earth.RotateAround (this.transform.position, new Vector3(0, 0.99f, 0), 30 * Time.deltaTime);
        	mercury.RotateAround (this.transform.position, new Vector3(0, 2.11f, 0), 47 * Time.deltaTime);
        	venus.RotateAround (this.transform.position, new Vector3(0, 3.23f, 0), 35 * Time.deltaTime);
        	mars.RotateAround (this.transform.position, new Vector3(0, 4.34f, 0), 24 * Time.deltaTime);
        	jupiter.RotateAround (this.transform.position, new Vector3(0, 1.02f, 0), 13 * Time.deltaTime);
        	saturn.RotateAround (this.transform.position, new Vector3(0, 0.98f, 0), 9 * Time.deltaTime);
        	uranus.RotateAround (this.transform.position, new Vector3(0, 0.97f, 0),  6 * Time.deltaTime);
        	neptune.RotateAround (this.transform.position, new Vector3(0, 0.96f, 0), 5 * Time.deltaTime);
        	earth.Rotate (Vector3.up * Time.deltaTime * 250);
        	mercury.Rotate (Vector3.up * Time.deltaTime * 300);
        	venus.Rotate (Vector3.up * Time.deltaTime * 280);
        	mars.Rotate (Vector3.up * Time.deltaTime * 220);
        	jupiter.Rotate (Vector3.up * Time.deltaTime * 180);
        	saturn.Rotate (Vector3.up * Time.deltaTime * 160);
        	uranus.Rotate (Vector3.up * Time.deltaTime * 150);
        	neptune.Rotate (Vector3.up * Time.deltaTime * 140);
    	}
}
```

---
# 编程实践
- 阅读以下游戏脚本

> Priests and Devils
Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!


程序需要满足的要求：
- play the game ( [http://www.flash-game.net/game/2535/priests-and-devils.html](http://www.flash-game.net/game/2535/priests-and-devils.html) )
- 列出游戏中提及的事物（Objects）
- 用表格列出玩家动作表（规则表），注意，动作越少越好
- 请将游戏中对象做成预制
- 在场景控制器 LoadResources 方法中加载并初始化 长方形、正方形、球 及其色彩代表游戏中的对象。
- 使用 C# 集合类型 有效组织对象
- 整个游戏仅 主摄像机 和 一个 Empty 对象， 其他对象必须代码动态生成！！！ 。 整个游戏不许出现 Find 游戏对象， SendMessage 这类突破程序结构的 通讯耦合 语句。 违背本条准则，不给分
- 请使用课件架构图编程，不接受非 MVC 结构程序
- 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！

>项目地址：[牧师与魔鬼](https://github.com/enyocloud/3dGameUnity/tree/master/HW3)

1. **游戏中提及的事物（Objects）：**
牧师、魔鬼、河岸、船、水
2. **玩家动作表（规则表）：**

玩家动作|执行条件|执行结果
-----|-----|-----
点击岸上角色|船靠近角色且有空位|角色上船
点击船上的角色||角色上岸
点击船|船上有角色|船开向对岸
点击Restart||重置游戏

3. **将游戏中对象做成预制**
![预制](https://img-blog.csdnimg.cn/20201005220045801.png#pic_center)



4. **动态加载初始化游戏对象**

```csharp
GameObject bank = Instantiate<GameObject>(Resources.Load("Prefabs/bank"), new Vector3(-12, -1, 0), Quaternion.identity) as GameObject;
bank.name=leftbank;
```
以此类推。

5. **MVC结构框**![MVC](https://img-blog.csdnimg.cn/20201005212353828.jpg?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
- MVC是模型(model)－视图(view)－控制器(controller)的缩写，它是一种软件设计模式，用一种业务逻辑、数据、界面显示分离的方法组织代码，将业务逻辑聚集到一个部件里面，在改进和个性化定制界面及用户交互的同时，不需要重新编写业务逻辑。
- 在牧师与恶魔游戏设计中，场景中的所有GameObject就是Model，它们受到Controller的控制，比如说牧师和魔鬼受到MyCharacterController类的控制，船受到BoatController类的控制，河岸受到CoastController类的控制。
- View就是UserGUI和ClickGUI，它们展示游戏结果，并提供用户交互的渠道（点击物体和按钮）。
- Controller：除了刚才说的MyCharacterController、BoatController、CoastController以外，还有更高一层的Controller：FirstController（场景控制器），FirstController控制着这个场景中的所有对象，包括其加载、通信、用户输入。
- 最高层的Controller是Director类，一个游戏中只能有一个实例，它控制着场景的创建、切换、销毁、游戏暂停、游戏退出等等最高层次的功能。

6. **具体代码分析：**
> 1. Director

```csharp
public class Director : System.Object {
    	private static Director _instance;
    	public SceneController currentSceneController { get; set; }

    	public static Director getInstance() {
        	if (_instance == null) {
            	_instance = new Director ();
        }
        return _instance;
    }
}

```
- Director是最高层的控制器，运行游戏时始终只有一个实例，它掌控着场景的加载、切换等，也可以控制游戏暂停、结束等等。
- Director类使用了单例模式。第一次调用Director.getInstance()时，会创建一个新的Director对象，保存在_instance，此后每次调用getInstance，都回返回_instance。也就是说Director最多只有一个实例。这样，我们在任何Script中的任何地方通过Director.getInstance()都能得到同一个Director对象，也就可以获得同一个currentSceneController，这样我们就可以轻易实现类与类之间的通信，比如说我在其他控制器中就可以使用Director.getInstance().somethingHappen()来告诉导演某一件事情发生了，导演就可以在somethingHappen()方法中做出对应的反应。

> 2. SceneController接口

```csharp
public interface SceneController {
    void loadResources ();
}

```
> 3. Moveable

```csharp
public class Moveable: MonoBehaviour {
    
    readonly float move_speed = 20;

    // change frequently
    int moving_status;  // 0->not moving, 1->moving to middle, 2->moving to dest
    Vector3 dest;
    Vector3 middle;

    void Update() {
        if (moving_status == 1) {
            transform.position = Vector3.MoveTowards (transform.position, middle, move_speed * Time.deltaTime);
            if (transform.position == middle) {
                moving_status = 2;
            }
        } else if (moving_status == 2) {
            transform.position = Vector3.MoveTowards (transform.position, dest, move_speed * Time.deltaTime);
            if (transform.position == dest) {
                moving_status = 0;
            }
        }
    }
    public void setDestination(Vector3 _dest) {
        dest = _dest;
        middle = _dest;
        if (_dest.y == transform.position.y) {  // boat moving
            moving_status = 2;
        }
        else if (_dest.y < transform.position.y) {  // character from coast to boat
            middle.y = transform.position.y;
        } else {                                // character from boat to coast
            middle.x = transform.position.x;
        }
        moving_status = 1;
    }

    public void reset() {
        moving_status = 0;
    }
}


```

- GameObject挂载上Moveable以后，Controller就可以通过setDestination()方法轻松地让GameObject移动起来。

>4. MyCharacterController



```csharp
public class MyCharacterController {
    readonly GameObject character;
    readonly Moveable moveableScript;
    readonly ClickGUI clickGUI;
    readonly int characterType; // 0->priest, 1->devil

    // change frequently
    bool _isOnBoat;
    CoastController coastController;


    public MyCharacterController(string which_character) {
        
        if (which_character == "priest") {
            character = Object.Instantiate (Resources.Load ("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            characterType = 0;
        } else {
            character = Object.Instantiate (Resources.Load ("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            characterType = 1;
        }
        moveableScript = character.AddComponent (typeof(Moveable)) as Moveable;

        clickGUI = character.AddComponent (typeof(ClickGUI)) as ClickGUI;
        clickGUI.setController (this);
    }

    public void setName(string name) {
        character.name = name;
    }

    public void setPosition(Vector3 pos) {
        character.transform.position = pos;
    }

    public void moveToPosition(Vector3 destination) {
        moveableScript.setDestination(destination);
    }

    public int getType() {  // 0->priest, 1->devil
        return characterType;
    }

    public string getName() {
        return character.name;
    }

    public void getOnBoat(BoatController boatCtrl) {
        coastController = null;
        character.transform.parent = boatCtrl.getGameobj().transform;
        _isOnBoat = true;
    }

    public void getOnCoast(CoastController coastCtrl) {
        coastController = coastCtrl;
        character.transform.parent = null;
        _isOnBoat = false;
    }

    public bool isOnBoat() {
        return _isOnBoat;
    }

    public CoastController getCoastController() {
        return coastController;
    }

    public void reset() {
        moveableScript.reset ();
        coastController = (Director.getInstance ().currentSceneController as FirstController).fromCoast;
        getOnCoast (coastController);
        setPosition (coastController.getEmptyPosition ());
        coastController.getOnCoast (this);
    }
}

```
- 在构造函数中实例化了一个perfab，创建GameObject，因此我们每new MyCharacterController()一次，场景中就会多一个游戏角色。
- 构造函数还将clickGUI挂载到了这个角色上，以监测“鼠标点击角色”的事件。

> 5. BoatController和CoastController

```csharp
/*CoastController*/
public class CoastController {
    readonly GameObject coast;
    readonly Vector3 from_pos = new Vector3(9,1,0);
    readonly Vector3 to_pos = new Vector3(-9,1,0);
    readonly Vector3[] positions;
    readonly int to_or_from;    // to->-1, from->1

    // change frequently
    MyCharacterController[] passengerPlaner;

    public CoastController(string _to_or_from) {
        positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0), 
            new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};

        passengerPlaner = new MyCharacterController[6];

        if (_to_or_from == "from") {
            coast = Object.Instantiate (Resources.Load ("Perfabs/Stone", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
            coast.name = "from";
            to_or_from = 1;
        } else {
            coast = Object.Instantiate (Resources.Load ("Perfabs/Stone", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
            coast.name = "to";
            to_or_from = -1;
        }
    }

    public int getEmptyIndex() {
        for (int i = 0; i < passengerPlaner.Length; i++) {
            if (passengerPlaner [i] == null) {
                return i;
            }
        }
        return -1;
    }

    public Vector3 getEmptyPosition() {
        Vector3 pos = positions [getEmptyIndex ()];
        pos.x *= to_or_from;
        return pos;
    }

    public void getOnCoast(MyCharacterController characterCtrl) {
        int index = getEmptyIndex ();
        passengerPlaner [index] = characterCtrl;
    }

    public MyCharacterController getOffCoast(string passenger_name) {   // 0->priest, 1->devil
        for (int i = 0; i < passengerPlaner.Length; i++) {
            if (passengerPlaner [i] != null && passengerPlaner [i].getName () == passenger_name) {
                MyCharacterController charactorCtrl = passengerPlaner [i];
                passengerPlaner [i] = null;
                return charactorCtrl;
            }
        }
        Debug.Log ("cant find passenger on coast: " + passenger_name);
        return null;
    }

    public int get_to_or_from() {
        return to_or_from;
    }

    public int[] getCharacterNum() {
        int[] count = {0, 0};
        for (int i = 0; i < passengerPlaner.Length; i++) {
            if (passengerPlaner [i] == null)
                continue;
            if (passengerPlaner [i].getType () == 0) {  // 0->priest, 1->devil
                count[0]++;
            } else {
                count[1]++;
            }
        }
        return count;
    }

    public void reset() {
        passengerPlaner = new MyCharacterController[6];
    }
}

/*BoatController*/
public class BoatController {
    readonly GameObject boat;
    readonly Moveable moveableScript;
    readonly ClickGUI clickGUI;
    readonly Vector3 fromPosition = new Vector3 (5, 1, 0);
    readonly Vector3 toPosition = new Vector3 (-5, 1, 0);
    readonly Vector3[] from_positions;
    readonly Vector3[] to_positions;

    // change frequently
    int to_or_from; // to->-1; from->1
    MyCharacterController[] passenger = new MyCharacterController[2];

    public BoatController() {
        to_or_from = 1;

        from_positions = new Vector3[] { new Vector3 (4.5F, 1.5F, 0), new Vector3 (5.5F, 1.5F, 0) };
        to_positions = new Vector3[] { new Vector3 (-5.5F, 1.5F, 0), new Vector3 (-4.5F, 1.5F, 0) };

        boat = Object.Instantiate (Resources.Load ("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
        boat.name = "boat";

        moveableScript = boat.AddComponent (typeof(Moveable)) as Moveable;
        clickGUI = boat.AddComponent (typeof(ClickGUI)) as ClickGUI;
    }


    public void Move() {
        if (to_or_from == -1) {
            moveableScript.setDestination(fromPosition);
            to_or_from = 1;
        } else {
            moveableScript.setDestination(toPosition);
            to_or_from = -1;
        }
    }

    public int getEmptyIndex() {
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger [i] == null) {
                return i;
            }
        }
        return -1;
    }

    public bool isEmpty() {
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger [i] != null) {
                return false;
            }
        }
        return true;
    }

    public Vector3 getEmptyPosition() {
        Vector3 pos;
        int emptyIndex = getEmptyIndex ();
        if (to_or_from == -1) {
            pos = to_positions[emptyIndex];
        } else {
            pos = from_positions[emptyIndex];
        }
        return pos;
    }

    public void GetOnBoat(MyCharacterController characterCtrl) {
        int index = getEmptyIndex ();
        passenger [index] = characterCtrl;
    }

    public MyCharacterController GetOffBoat(string passenger_name) {
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger [i] != null && passenger [i].getName () == passenger_name) {
                MyCharacterController charactorCtrl = passenger [i];
                passenger [i] = null;
                return charactorCtrl;
            }
        }
        Debug.Log ("Cant find passenger in boat: " + passenger_name);
        return null;
    }

    public GameObject getGameobj() {
        return boat;
    }

    public int get_to_or_from() { // to->-1; from->1
        return to_or_from;
    }

    public int[] getCharacterNum() {
        int[] count = {0, 0};
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger [i] == null)
                continue;
            if (passenger [i].getType () == 0) {    // 0->priest, 1->devil
                count[0]++;
            } else {
                count[1]++;
            }
        }
        return count;
    }

    public void reset() {
        moveableScript.reset ();
        if (to_or_from == -1) {
            Move ();
        }
        passenger = new MyCharacterController[2];
    }
}

```

>6. UserAction

```csharp
public interface UserAction {
    void moveBoat();
    void characterIsClicked(MyCharacterController characterCtrl);
    void restart();
}
```
- 接口实际上使用了门面模式：FirstController必须要实现这个接口才能对用户的输入做出反应。
- 好处：通过一套接口（UserAction）来定义Controller与GUI交互的渠道，这样实现Controller类的程序员只需要实现UserAction接口，他的代码就可以被任何支持这个接口的GUI类所使用；实现GUI类的程序员也不需要知道Controller的实现方式，它只需要调用接口中的方法，后面的事情交给Controller。

>7. ClickGUI

```csharp
public class ClickGUI : MonoBehaviour {
    UserAction action;
    MyCharacterController characterController;

    public void setController(MyCharacterController characterCtrl) {
        characterController = characterCtrl;
    }

    void Start() {
        action = Director.getInstance ().currentSceneController as UserAction;
    }

    void OnMouseDown() {
        if (gameObject.name == "boat") {
            action.moveBoat ();
        } else {
            action.characterIsClicked (characterController);
        }
    }
}

```
ClickGUI类是用来监测用户点击，并调用SceneController进行响应的。

7. **游戏预览截图**
> 游戏开始

![游戏1](https://img-blog.csdnimg.cn/20201005221521419.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

> 游戏失败

![游戏2](https://img-blog.csdnimg.cn/2020100522160168.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
> 游戏成功

![游戏3](https://img-blog.csdnimg.cn/20201005221621411.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)

---


# 思考题【选做】

**使用向量与变换，实现并扩展 Tranform 提供的方法，如 Rotate、RotateAround 等。**

1. 直接改变对象的位置：

```csharp
void Update () {
  	float delta = Mathf.Atan (this.transform.position.y / this.transform.position.x);
  	float temp_x = this.transform.position.x;
  	float temp_y = this.transform.position.y;
  	this.transform.position.x = Mathf.Cos (delta) * temp_x - Mathf.Sin (delta) * temp_y;
  	this.transform.position.y = Mathf.Sin (delta) * temp_x + Mathf.Cos (delta) * temp_y;
}

```

2. 通过Vector3向量来实现：

```csharp
public int angle;
void Update () {
      	float delta = Mathf.Atan (this.transform.position.y / this.transform.position.x);
  	angle -= delta;
  	transform.Translate(new Vector3(Time.deltaTime * 10*Mathf.Cos(angle), Time.deltaTime * 10*Mathf.Sin(angle), 0));  
}

```

