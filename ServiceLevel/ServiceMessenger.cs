using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using DataLevel;
using Model;

namespace ServiceLevel
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceMessenger" в коде и файле конфигурации.
    public class ServiceMessenger : IServiceMessenger
    {
        static ClockworkDataBase db = new ClockworkDataBase();
        public int GetCompanion(string nick)
        {
            return db.GetUserId(nick);
        }
    }
}
