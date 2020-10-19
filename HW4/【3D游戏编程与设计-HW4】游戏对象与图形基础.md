@[TOC](游戏对象与图形基础)

---
# 基本操作演练

- **下载 Fantasy Skybox FREE， 构建自己的游戏场景**
1. 通过asset store下载skybox
2. import 进 project 目录
![1](https://img-blog.csdnimg.cn/20201019203029128.png#pic_center)

3. 创建一个新的material，在inspector中选择为shader-skybox-6sides
4. 选择下载到的贴图文件，拖放到对应的位置中
5. 将对应的material添加到camera中
6. 结果如下图所示
![2](https://img-blog.csdnimg.cn/2020101920331359.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)


- **写一个简单的总结，总结游戏对象的使用**
- unity中的游戏对象是 unity 游戏中的基本单位，可以是 camera、light ，也可以是 gameobject、UI 等。通过对对象的交互，可以实现一个游戏的功能。 
- 游戏对象的使用一般是通过 c# 的 script 来完成，c# 可以控制游戏对象中的行为，游戏对象的创建与销毁。 
- 更多的，我们采用的是通过 MVC 模式来构建游戏，Model（模型）是应用程序中用于处理应用程序数据逻辑的部分。通常模型对象负责在数据库中存取数据。
- View（视图）是应用程序中处理数据显示的部分。通常视图是依据模型数据创建的。
- Controller（控制器）是应用程序中处理用户交互的部分。通常控制器负责从视图读取数据，控制用户输入，并向模型发送数据。 
- 同时我们可以更加细分的对控制器进行分配职能，例如设置动作管理器等方式来保持控制器的简洁性和可扩展性。


---
# 编程实践

- **牧师与魔鬼 动作分离版【2019开始的新要求】：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束**

项目地址：[https://github.com/enyocloud/3dGameUnity/tree/master/HW4](https://github.com/enyocloud/3dGameUnity/tree/master/HW4)

改进目的：
1. 把每个需要移动的游戏对象的移动方法提取出来，建立一个动作管理器来管理不同的移动方法。
2. 对于上一个版本，每一个可移动的游戏对象的组件都有一个Move脚本，当游戏对象需要移动时候，游戏对象自己调用Move脚本中的方法让自己移动。而动作分离版，则剥夺了游戏对象自己调用动作的能力，建立一个动作管理器，通过场景控制器(在我的游戏中是Controllor)把需要移动的游戏对象传递给动作管理器，让动作管理器去移动游戏对象。
3. 将物体的动作与空间属性分开来，从而降低耦合，易于开发者维护。当动作很多或是需要做同样动作的游戏对象很多的时候，使用动作管理器可以让动作很容易管理，也提高了代码复用性。

具体编程设计如下：

**动作管理类图描述与实现**
- ISSActionCallback（动作事件接口） 
> 作为动作和动作管理者的接口（组合动作也可以是动作管理者），动作管理者继承这个接口，并且实现接口的方法。当动作完成的时候，动作会调用这个接口，发送消息告诉动作管理者对象这个动作做完，然后管理者会对下一个动作进行处理。

```csharp
public enum SSActionEventType : int { Started, Competeted }  

public interface ISSActionCallback  
{  
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
        int intParam = 0, string strParam = null, Object objectParam = null);  
}

```

- SSActionManager（动作管理基类） 
> 管理SequenceAction和SSAction，可以给它们传递游戏对象，让游戏对象做动作或是一连串的动作，控制动作的切换。SSActionManager继承了ISSActionCallback接口，通过这个接口，当动作做完或是连续的动作做完时候会告诉SSActionManager，然后SSActionManager去决定如何执行下一个动作。

```csharp
public class SSActionManager: MonoBehaviour, ISSActionCallback
{  
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //将执行的动作的字典集合,int为key，SSAction为value
    private List<SSAction> waitingAdd = new List<SSAction>();                       //等待去执行的动作列表
    private List<int> waitingDelete = new List<int>();                              //等待删除的动作的key    
                
    protected void Update() 
    {
        foreach(SSAction ac in waitingAdd)                                
        {
            actions[ac.GetInstanceID()] = ac;                                      //获取动作实例的ID作为key
        }
        waitingAdd.Clear();
        foreach(KeyValuePair<int, SSAction> kv in actions) 
        {
            SSAction ac = kv.Value;
            if (ac.destroy)          //如果动作被标记摧毁，那么加入等待删除列表
            {
                waitingDelete.Add(ac.GetInstanceID());
            } 
            else if (ac.enable) 
            {
                ac.Update();
            }
        }
        foreach(int key in waitingDelete) 
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }
    
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) 
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;                                               
        waitingAdd.Add(action);                                           
        action.Start();
    }
    
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        //牧师与魔鬼的游戏对象移动完成后就没有下一个要做的动作了，所以回调函数为空
    }
}
```
- SequenceAction（组合动作实现） 

> SequenceAction继承了ISSActionCallback，因为组合动作是每一个动作的顺序完成，它管理这一连串动作中的每一个小的动作，所以当小的动作完成的时候，也要发消息告诉它，然后它得到消息后去处理下一个动作。SequenceAction也继承了SSAction，因为成个组合动作也需要游戏对象，也需要标识是否摧毁，也会有一个组合动作的管理者的接口，组成动作也是动作的子类，只不过是让具体的动作组合起来做罢了。

```csharp
public class SequenceAction: SSAction, ISSActionCallback
{           
    public List<SSAction> sequence;    //动作的列表
    public int repeat = -1;            //-1就是无限循环做组合中的动作
    public int start = 0;              //当前做的动作的索引
    
    public static SequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence) 
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();//让unity自己创建一个SequenceAction实例
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }
    
    public override void Update() 
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count) 
        {
            sequence[start].Update();     //一个组合中的一个动作执行完后会调用接口,所以这里看似没有start++实则是在回调接口函数中实现
        }
    }
   
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,  
        int intParam = 0, string strParam = null, Object objectParam = null)
    {
        source.destroy = false;     //先保留这个动作，如果是无限循环动作组合之后还需要使用
        this.start++;
        if (this.start >= sequence.Count) 
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0) 
            {
                this.destroy = true;               
                this.callback.SSActionEvent(this); //告诉组合动作的管理对象组合做完了
            }
        }
    }
    
    public override void Start() 
    {
        foreach(SSAction action in sequence) 
        {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;                //组合动作的每个小的动作的回调是这个组合动作
            action.Start();
        }
    }
    
    void OnDestroy() 
    {
        //如果组合动作做完第一个动作突然不要它继续做了，那么后面的具体的动作需要被释放
    }
}
```
- SSAction （动作基类） 

> SSAction是所有动作的基类。SSAction继承了ScriptableObject代表SSAction不需要绑定GameObject对象，且受Unity引擎场景管理。

```csharp
public class SSAction : ScriptableObject            //动作
{
    public bool enable = true;                      //是否正在进行此动作
    public bool destroy = false;                    //是否需要被销毁
    public GameObject gameobject;                   //动作对象
    public Transform transform;                     //动作对象的transform
    public IS
    protected SSAction() {}                        //保证SSAction不会被new
    //子类可以使用下面这两个函数
    public virtual void Start()                     
    {
        throw new System.NotImplementedException();
    }
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}
```
- SSMoveToAction（移动动作实现）

```csharp
public class SSMoveToAction : SSAction                        //移动
{
    public Vector3 target;        //移动到的目的地
    public float speed;           //移动的速度
    private SSMoveToAction(){}
    public static SSMoveToAction GetSSAction(Vector3 target, float speed) 
    {
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();//让unity自己创建一个MoveToAction实例，并自己回收
        action.target = target;
        action.speed = speed;
        return action;
    }
    public override void Update() 
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed*Time.deltaTime);
        if (this.transform.position == target) 
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);      //告诉动作管理或动作组合这个动作已完成
        }
    }
    public override void Start() 
    {
        //移动动作建立时候不做任何事情
    }
}

```
- MySceneActionManager（移动动作管理实现）
> 船的移动是一个SSMoveToAction动作就可以，而角色的移动需要两个SSMoveToAction动作组合（先垂直后水平移动或先水平后垂直移动）。然后设置当前场景控制器的动作管理者为MySceneActionManager，这样场景控制器就可以调用动作管理器的方法实现不同游戏对象的移动啦。

```csharp
public class MySceneActionManager:SSActionManager    
{ 
    public SSMoveToAction moveBoatToEndOrStart;     //移动船到结束岸，移动船到开始岸
    public SequenceAction moveRoleToLandorBoat;     //移动角色到陆地，移动角色到船上
    public Controllor sceneController;             //当前场景的场景控制器
    protected new void Start()
    {
        sceneController = (Controllor)SSDirector.GetInstance().CurrentScenceController;
        sceneController.actionManager = this;     //设置该场景控制器的动作管理者为自己
    }
    public void moveBoat(GameObject boat, Vector3 target, float speed)
    {
        moveBoatToEndOrStart = SSMoveToAction.GetSSAction(target, speed);
        this.RunAction(boat, moveBoatToEndOrStart, this);
    }
    public void moveRole(GameObject role, Vector3 middle_pos, Vector3 end_pos,float speed)
    {
        SSAction action1 = SSMoveToAction.GetSSAction(middle_pos, speed);
        SSAction action2 = SSMoveToAction.GetSSAction(end_pos, speed);
        moveRoleToLandorBoat = SequenceAction.GetSSAcition(1, 0, new List<SSAction>{action1, action2}); //1是做一次动作组合，0代表从action1开始
        this.RunAction(role, moveRoleToLandorBoat, this);
    }
}   
```
- Controllor（当前场景控制器）

> 最后Controllor接收到游戏对象需要移动的时候，只要传给MySceneActionManager相应的参数和游戏对象就可以进行移动.
> SSActionManager继承了MonoBehaviour。MonoBehaviour是每个脚本的基类，又继承于Behaviours。只有继承了MonoBehaviour的类才可以作为组件挂载到游戏对象上，在unity 中所有继承MonoBehaviour的类是不可以实例化的，因为unity都会自动为其创建实例，所以我们需要调用该类的时候不使用new，而是使用AddComponent来调用。

```csharp
public MySceneActionManager actionManager;
void Start ()
{
    SSDirector director = SSDirector.GetInstance();
    director.CurrentScenceController = this;
    user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
    LoadResources();
    actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager;    
}
public void MoveBoat()                  //移动船
{
    if (boat.IsEmpty() || user_gui.sign != 0) return;
    actionManager.moveBoat(boat.getGameObject(),boat.BoatMoveToPosition(),boat.move_speed);   //动作管理器实现移动船
    user_gui.sign = Check();
    if (user_gui.sign == 1)
    {
        for (int i = 0; i < 3; i++)
        {
            roles[i].PlayGameOver();
            roles[i + 3].PlayGameOver();
        }
    }
}
public void MoveRole(RoleModel role)    //移动角色
{
    if (user_gui.sign != 0) return;
    if (role.IsOnBoat())
    {
        LandModel land;
        if (boat.GetBoatSign() == -1)
            land = end_land;
        else
            land = start_land;
        boat.DeleteRoleByName(role.GetName());
        //动作分离版本改变
        Vector3 end_pos = land.GetEmptyPosition();                          
        Vector3 middle_pos = new Vector3(role.getGameObject().transform.position.x, end_pos.y, end_pos.z);  
        actionManager.moveRole(role.getGameObject(), middle_pos, end_pos, role.move_speed);  
        role.GoLand(land);
        land.AddRole(role);
    }
    else
    {                                
        LandModel land = role.GetLandModel();
        if (boat.GetEmptyNumber() == -1 || land.GetLandSign() != boat.GetBoatSign()) return;   //船没有空位，也不是船停靠的陆地，就不上船
        land.DeleteRoleByName(role.GetName());
        //动作分离版本改变
        Vector3 end_pos = boat.GetEmptyPosition();  
        Vector3 middle_pos = new Vector3(end_pos.x, role.getGameObject().transform.position.y, end_pos.z); 
        actionManager.moveRole(role.getGameObject(), middle_pos, end_pos, role.move_speed);  
        role.GoBoat(boat);
        boat.AddRole(role);
    }
    user_gui.sign = Check();
    if (user_gui.sign == 1)
    {
        for (int i = 0; i < 3; i++)
        {
            roles[i].PlayGameOver();
            roles[i + 3].PlayGameOver();
        }
    }
}
```
- **设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束**
```csharp
public class Judger
{
    LandModel start_land;
    LandModel end_land;
    BoatModel boat; 
    public Judger(LandModel bl, LandModel el, BoatModel b) {
        start_land = bl;
        end_land = el;
        boat = b;
    }
    public int Check() {
        int start_priest = (start_land.GetRoleNum())[0];
        int start_devil = (start_land.GetRoleNum())[1];
        int end_priest = (end_land.GetRoleNum())[0];
        int end_devil = (end_land.GetRoleNum())[1];
        if (end_priest + end_devil == 6)        // 获胜
            return 3;
        int[] boat_role_num = boat.GetRoleNum();
        if (boat.GetBoatSign() == 1) {          // 在开始岸和船上的角色
            start_priest += boat_role_num[0];
            start_devil += boat_role_num[1];
        } else {                                // 在结束岸和船上的角色
            end_priest += boat_role_num[0];
            end_devil += boat_role_num[1];
        }
        if ((start_priest > 0 && start_priest < start_devil) || (end_priest > 0 && end_priest < end_devil)) { //失败
            return 2;
        }
        return 1;                               //未完成
    }
}
```
- **实现效果和上次一样，也是打包好只用替换Assert，具体如下图所示：**
>游戏开始：

![3](https://img-blog.csdnimg.cn/20201019212017733.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
> 游戏失败：

![4](https://img-blog.csdnimg.cn/20201019212120746.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)
> 游戏成功：

![5](https://img-blog.csdnimg.cn/20201019212947123.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2plc3NpYWZsb3Jh,size_16,color_FFFFFF,t_70#pic_center)



---
