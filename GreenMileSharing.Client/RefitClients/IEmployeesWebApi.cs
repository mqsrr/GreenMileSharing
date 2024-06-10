using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

[Headers("Bearer")]
internal interface IEmployeesWebApi
{

    [Get(ApiEndpoints.Employee.GetByUsername)]
    Task<IApiResponse<Employee>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}