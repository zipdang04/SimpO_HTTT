using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleSockets.Messaging.Metadata;

namespace Server.Information
{
	public class PlayerNetwork
	{
		public static readonly int PLAYER = 4;
		public IClientInfo[] clients = new IClientInfo[4];

		public PlayerNetwork() {
			IClientInfo[] clients = new IClientInfo[4];
		}

		public bool connect(int posi, IClientInfo client)
		{
			if (clients[posi] != null) return false;
			clients[posi] = client;
			return true;
		} 
		public void disconnect(IClientInfo client)
		{
			for (int i = 0; i < PLAYER; i++) {
				try {
					if (clients[i] != null && clients[i].Id == client.Id) {
						clients[i] = null;
						break;
					}
				} catch { }
			}
		}
		public int findPositionFromId(IClientInfo client)
		{
			for (int i = 0; i < PLAYER; i++) {
				try {
					if (clients[i] != null && clients[i].Id == client.Id)
						return i;
				} catch { }
			}
			return -1;
		}
	}
}
