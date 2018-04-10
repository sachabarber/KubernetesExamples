using ServiceStack;
using System.Collections.Generic;

namespace sswebapp.ServiceModel
{
    [Route("/showsettings","GET")]
    public class ShowSettingRequest : IReturn<ShowSettingResponse>
    {
        
    }

    public class ShowSettingResponse
    {
        public IEnumerable<string> Results { get; set; }
    }
    
}
