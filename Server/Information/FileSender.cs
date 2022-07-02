using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleSockets.Server;
using Server.Information;
using Server.QuestionClass;
using SimpleSockets.Messaging.Metadata;
using SimpleSockets;
using Server.HostServer.Components;
using System.IO;

namespace Server.Information
{
	public class FileSender
	{
		const int PACKET_SIZE = 1048576;
		const int MAX_CNT = 2048;
		SimpleSocketListener listener;
		public delegate void Percentage(int cur, int cnt);
		public FileSender(SimpleSocketListener listener)
		{
			this.listener = listener;
		}
		async Task sendMessageToEveryone(string message)
		{
			await Task.Run(() => { 
				foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
				{
					listener.SendMessage(client.Value.Id, message);
				}
			});
		}
		async Task sendBytesToEveryone(byte[] bytes)
		{
			await Task.Run(() => {
				foreach (KeyValuePair<int, IClientInfo> client in listener.GetConnectedClients())
				{
					listener.SendBytes(client.Value.Id, bytes);
				}
			});
		}

		public async Task Send(string path)
		{
			byte[] bytes = new byte[1];
			await Task.Run(() => { bytes = File.ReadAllBytes(path); });

			IEnumerable<byte[]> chunks = bytes.Chunk(PACKET_SIZE);
			int cnt = chunks.Count();

			await sendMessageToEveryone(string.Format("OLPA SEND {0}", cnt));
			foreach (byte[] cur in chunks)
			{
				await sendBytesToEveryone(cur);
			}
		}
	}
}
