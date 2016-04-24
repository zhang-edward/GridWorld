using UnityEngine;
using System.Collections;

/* No need for WorldTile object??
 * 
 * if reference to Entity, TerrainObject, TerrainStatus, or TerrainBiome is needed,
 * just use the layers in the World class?
 * */

namespace Obsolete
{
	public class WorldTile {
		
		public int row;
		public int col;
		
		public Entity entity;
		public TerrainObject terrainObject;
		
		public WorldTile(int r, int c, Entity ent, TerrainObject tObj)
		{
			row = r;
			col = c;
			entity = ent;
			terrainObject = tObj;
		}
		
		// Instantiate the gameobjects for the tile
		public void InitTile()
		{
			Debug.Log ("Initing tile " + entity.gameObject + " and " + terrainObject.gameObject);
			//Vector3 worldPos = new Vector3(col, row);	// col = x and row = y
		}
	}
}
