namespace CGT
{
    public interface IOrderableBehaviour
    {
        int Priority { get; }
        void AwakeInit();
        void StartInit();
        void OnEarlyUpdate();
        void OnUpdate();
        void OnLateUpdate();
        void OnFixedUpdate();
        bool Enabled { get; set; }
    }
}