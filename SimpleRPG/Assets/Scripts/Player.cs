using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public static Player instance;
	public IControllable controlledEntity;

	public UnityEvent OnChangeEntityControl;

	public EntityInfo entityInfo;

    public World world;
	
	void Awake()
	{
		instance = this;
    }

	void Update()
	{   
        // if clicked on entity that is controllable, begin control of that entity
        if (Input.GetMouseButtonDown(0))
        {
			Debug.Log("hello");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePos.x) + World.WORLD_SIZE / 2;
            int y = Mathf.RoundToInt(mousePos.y) + World.WORLD_SIZE / 2;
            if (world.isInBounds(x, y) && world.EntityExistsAt(x, y))
            {
				Debug.Log(world.getEntity(x, y));
                IControllable clicked = world.getEntity(x, y) as IControllable;
                if (clicked != null)
                {
                    // release control of currently controlled entity (prevent controlling >1 entity at a time)
                    if (controlledEntity != null)
                        controlledEntity.ReleaseControl();

                    controlledEntity = clicked;
                    controlledEntity.InitControl();
					OnChangeEntityControl.Invoke();
                }
            }
        }

        if (controlledEntity != null)
            HandleEntityControl();
	}

    public void HandleEntityControl()
    {
        // if controlled entity dies
        if (!controlledEntity.Controllable())
        {
            controlledEntity.ReleaseControl();
            controlledEntity = null;
        }

        // the position index for executing commands
        int pos;

        // Directional keys input
        if (Input.GetKeyDown(KeyCode.UpArrow))
            pos = 0;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            pos = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            pos = 2;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            pos = 3;
        else
            pos = -1;

		if (pos != -1)
		{
			int dx = 0;
			int dy = 0;
			if (pos == 0)
				dy = 1;
			else if (pos == 1)
				dx = 1;
			else if (pos == 2)
				dy = -1;
			else if (pos == 3)
				dx = -1;
			int xInspect = controlledEntity.GetEntity().x + dx;
			int yInspect = controlledEntity.GetEntity().y + dy;
			TryInspectPosition(xInspect, yInspect);
			// if shift is not held down, execute a command
			if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				// have the entity execute the command for that direction
				controlledEntity.ExecuteCommand(pos);
			}
		}
    }

	private void TryInspectPosition(int x, int y)
	{
		if (world.isInBounds(x, y))
		{
			LivingEntity e = world.getEntity(x, y) as LivingEntity;
			if (e != null)
			{
				entityInfo.InitInspection(e);
			}
		}
	}
}
