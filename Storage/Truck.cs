using UnityEngine;

public class Truck : MonoBehaviour
{

    public TaskData TaskData { get; private set; }

    public void LoadCargo(TaskData taskData)
    {
        TaskData = taskData;
    }

    public void UnloadCargo()
    {
        Destroy(gameObject);
    }

}
