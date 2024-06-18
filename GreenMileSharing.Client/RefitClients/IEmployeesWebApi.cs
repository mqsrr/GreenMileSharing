using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenMileSharing.Client.Helpers;
using GreenMileSharing.Client.Models;
using Refit;

namespace GreenMileSharing.Client.RefitClients;

[Headers("Bearer")]
internal interface IEmployeesWebApi
{

    [Get(ApiEndpoints.Employee.GetAll)]
    Task<IApiResponse<IEnumerable<Employee>>> GetAllAsync(string apiVersion, CancellationToken cancellationToken);
    
    [Get(ApiEndpoints.Employee.GetByUsername)]
    Task<IApiResponse<Employee>> GetByUsernameAsync(string apiVersion, string username, CancellationToken cancellationToken);
    
    [Delete(ApiEndpoints.Employee.DeleteById)]
    Task<IApiResponse> DeleteByIdAsync(string apiVersion, Guid id, CancellationToken cancellationToken);
}