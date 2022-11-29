using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Networking
{
    interface IWebApiClient
    {
        Task<FetchResult> Fetch(string cloudProjectId, string environmentId);
        Task<string> Post(string cloudProjectId, string environmentId, JArray configs);
        Task Put(string cloudProjectId, string environmentId, string configId, JArray configs);
    }
}
