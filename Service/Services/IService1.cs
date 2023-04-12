using System.Threading.Tasks;

namespace Service1
{
    public interface IService1
    {
        public Task<ApplesPayloadModelOut> CallSomeService(string query);
    }
}
