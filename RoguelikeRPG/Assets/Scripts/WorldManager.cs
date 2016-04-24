using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public World world;
	public enum WorldTurn {
		Player,
		Enemy,
		Status
	}
	public Player player;

	public WorldTurn turn;

	void Awake()
	{
		world = GetComponent <World>();
	}

	void Start()
	{
		// TODO: Make this more generic later
		player = world.player;
	}

	// Update is called once per frame
	void Update () {

		if (turn == WorldTurn.Player)
		{
			GetPlayerInput();
		}

		if (turn == WorldTurn.Enemy)
		{
			foreach (Entity e in world.entityLayer)
			{
				if (e.tag != "Player")
				{
					e.Act();
				}
			}
			turn = WorldTurn.Player;
		}

	}
	
	void GetPlayerInput()
	{
		Vector3 dest;
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			/*player.pos.x --;
			player.transform.position = player.pos;*/

			dest = new Vector3(player.x - 1, player.y);
			player.AttemptMove(dest);
			turn = WorldTurn.Enemy;
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			/*player.pos.x ++;
			player.transform.position = player.pos;*/

			dest = new Vector3(player.x + 1, player.y);
			player.AttemptMove(dest);
			turn = WorldTurn.Enemy;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			/*player.pos.y ++;
			player.transform.position = player.pos;*/

			dest = new Vector3(player.x, player.y + 1);
			player.AttemptMove(dest);
			turn = WorldTurn.Enemy;
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			/*player.pos.y --;
			player.transform.position = player.pos;*/

			dest = new Vector3(player.x, player.y - 1);
			player.AttemptMove(dest);
			turn = WorldTurn.Enemy;
		}
	}
}
