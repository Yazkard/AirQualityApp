using AirQualityApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityApp.Services
{
    interface IRestService
    {
        Task<List<StationData>> RefreshDataAsync(BoundingBox boundingBox);

        Task SaveStationDataAsync(StationData item, bool isNewItem);

        Task DeleteStationDataAsync(string id);
    }
}
