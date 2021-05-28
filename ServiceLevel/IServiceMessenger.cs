using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceLevel
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IServiceMessenger" в коде и файле конфигурации.
    [ServiceContract]
    public interface IServiceMessenger
    {
        [OperationContract]
        int GetCompanion(string nick);
    }
}
