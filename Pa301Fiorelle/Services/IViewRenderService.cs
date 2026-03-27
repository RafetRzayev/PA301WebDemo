using System.Threading.Tasks;

namespace Pa301Fiorelle.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
