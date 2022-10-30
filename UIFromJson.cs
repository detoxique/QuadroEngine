using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using QuadroEngine.UI;

namespace QuadroEngine
{
    class UIFromJson
    {
        public UI_Element Deserialize()
        {
            return JsonConvert.DeserializeObject<UI_Element>(@"Designer\UIs");
        }
    }
}
