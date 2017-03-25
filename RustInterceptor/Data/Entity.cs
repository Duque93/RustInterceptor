using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rust_Interceptor.Data {

	public class Entity {
		uint networkOrder = 0;
		internal ProtoBuf.Entity proto;
		public ProtoBuf.Entity Data {
			get { return proto; }
			private set { proto = value; }
		}
		public bool IsPlayer { get { return proto.basePlayer != null; } }
		public bool IsLocalPlayer { get { return proto.basePlayer.metabolism != null; } }

		public UInt32 UID { get { return proto.baseNetworkable.uid; } private set { proto.baseNetworkable.uid = value; } }
		public Vector3 Position { get { return proto.baseEntity.pos;} private set { proto.baseEntity.pos = value; } }
		public Vector3 Rotation { get { return proto.baseEntity.rot; }private set { proto.baseEntity.rot = value; } }

		static Dictionary<UInt32, Entity> entities = new Dictionary<uint, Entity>();
		public static Entity GetLocalPlayer() {
			return First(item => item.Value.IsLocalPlayer);
		}

		public static List<Entity> GetPlayers() {
			return Find(item => { return item.Value.IsPlayer; });
		}

		public static bool Has(UInt32 uid) {
			return entities.ContainsKey(uid);
		}

		public static Entity First(Func<KeyValuePair<UInt32, Entity>, bool> predicate) {
			lock (entities) {
				return entities.First(predicate).Value;
			}
		}

		public static List<Entity> Find(Func<KeyValuePair<UInt32, Entity>, bool> predicate) {
			IEnumerable<KeyValuePair<UInt32, Entity>> results;
			lock (entities) {
				results = entities.Where(predicate);
			}
			return (from item in results select item.Value).ToList();
		}

		public static Entity Find(UInt32 uid) {
			Entity ent;
			lock (entities) {
				if (entities.TryGetValue(uid, out ent)) {
					return ent;
				}
			}
			return null;
		}

		public static Entity CreateOrUpdate(UInt32 networkOrder, ProtoBuf.Entity entityInfo) {
			uint uid = entityInfo.baseNetworkable.uid;
			if (Has(uid)) {
				Entity entity;
				lock (entities) {
					entity = entities[uid];
				}
				entity.networkOrder = networkOrder;
				entity.proto = entityInfo;
				lock (entities) {
					entities[uid] = entity;
				}
				return entity;
			} else {
				Entity entity = new Entity();
				entity.networkOrder = networkOrder;
				entity.proto = entityInfo;
				lock (entities) {
					entities.Add(uid, entity);
				}
				return entity;
			}
		}

		public static void CreateOrUpdate(EntityDestroy destroyInfo) {
			lock (entities) {
				if (Has(destroyInfo.UID))
					entities.Remove(destroyInfo.UID);
			}
		}

		public static Entity UpdatePosistion(Data.Entity.EntityUpdate update) {
			if (!Has(update.uid)) return null;
			Entity entity;
			lock (entities) {
				entity = entities[update.uid];
			}
			entity.Position = update.position;
			entity.Rotation = update.rotation;
			lock (entities) {
				entities[update.uid] = entity;
			}
			return entity;
		}

		public static List<Entity> UpdatePositions(List<Data.Entity.EntityUpdate> updates) {
			List<Entity> entities = new List<Entity>();
			foreach (var update in updates) {
				var entity = UpdatePosistion(update);
				if (entity != null) entities.Add(entity);
			}
			return entities.Count > 0 ? entities : null;
		}

		public static List<EntityUpdate> ParsePositions(Packet p) {
			List<EntityUpdate> updates = new List<EntityUpdate>();
			/* EntityPosition packets may contain multiple positions */
			while (p.unread >= 28L /* Uint32 = 4bytes, Float = 4bytes. Uint32 + (Float * 6) = 28 */) {
				EntityUpdate update = new EntityUpdate();
				/* Entity UID */
				update.uid = p.UInt32();
				/* Read 2 Vector3, Position and Rotation */
				update.position = p.Vector3();
				update.rotation = p.Vector3();
				updates.Add(update);
			}
			return updates;
		}

		public static uint ParseEntity(Packet p, out ProtoBuf.Entity entity) {
			/* Entity Number/Order */
			var num = p.UInt32();
			entity = ProtoBuf.Entity.Deserialize(p);
			return num;
		}

		public class EntityUpdate {
			internal uint uid;
			public uint UID { get { return uid; } }
			internal Vector3 position;
			public Vector3 Position { get { return position; } }
			internal Vector3 rotation;
			public Vector3 Rotation { get { return rotation; } }
		}


	}
}
