//敌人状态接口类
public interface IState
{
    void OnEnter(); //进入接口

    void OnUpdate();//执行时

    void OnExit();//退出时
}
