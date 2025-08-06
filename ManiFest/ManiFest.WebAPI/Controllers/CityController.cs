using ManiFest.Model.Requests;
using ManiFest.Model.Responses;
using ManiFest.Model.SearchObjects;
using ManiFest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManiFest.WebAPI.Controllers
{
    public class CityController : BaseCRUDController<CityResponse, CitySearchObject, CityUpsertRequest, CityUpsertRequest>
    {
        public CityController(ICityService service) : base(service)
        {
        }
    }
} 