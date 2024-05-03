using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Windows.Controls;
namespace wcf_chat
{
  
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
			ServerUser user = new ServerUser()
			{
				ID = nextId,
				Name = name,
				operationContext = OperationContext.Current
			};
			nextId++;

			SendMsg(": " + user.Name + " подключился к сервису!", 0);
			Console.WriteLine(DateTime.Now.ToShortTimeString() + ": <" + user.Name + "> присоединился к сервису!" + "|ID пользователя: " + user.ID);
			users.Add(user);
			return user.ID;
		}
        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user!=null)
            {
                users.Remove(user);
                SendMsg(": "+user.Name + ": " + " покинул сервис!",0);
				Console.WriteLine(DateTime.Now.ToShortTimeString() + ": <" + user.Name + "> покинул чат!" + "|ID пользователя: " + user.ID);
			}
        }

        public void SendMsg(string msg, int id)
        {
            if (id == 0)
            {
                Console.WriteLine("Ответ с сервера получен: " + msg);
            }
            else
            {
                foreach (var item in users)
                {
                    string answer = DateTime.Now.ToShortTimeString();

                    var user = users.FirstOrDefault(i => i.ID == id);
                    if (user != null)
                    {
                        answer += ": " + user.Name + " ";
                    }
                    answer += msg;
                    Console.WriteLine(user.Name + " отправил сообщение" + ": " + msg);
                    item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
                }
            }
        }


		
	}
}
