using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IMailSender
    {
        public Task SendAsync(List<string> to, string subject, string date);
        public Task SendFromHtmlAsync(List<string> to, string subject, string file);
    }
}