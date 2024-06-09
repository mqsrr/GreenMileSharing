using System;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

[Headers("Bearer")]
internal interface IEmployeesWebApi
{
    [Get(ApiEndpoints.Employee.GetById)]
    Task<IApiResponse<Employee>> GetByIdAsync(Guid id);

    [Get(ApiEndpoints.Employee.GetByUsername)]
    Task<IApiResponse<Employee>> GetByUsernameAsync(string username);

    [Put(ApiEndpoints.Employee.UpdateUsername)]
    Task<IApiResponse> UpdateUsername(Guid id, [Query] string username);
    
    [Delete(ApiEndpoints.Employee.DeleteById)]
    Task<IApiResponse> DeleteById(Guid id);
}