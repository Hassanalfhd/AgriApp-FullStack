using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Dtos.OrdersDtos;
using Agricultural_For_CV_Shared.Enums;
using Agricultural_For_CV_Shared.Results;

namespace Agricultural_For_CV_Shared.Interfaces
{
    public interface IFarmerService
    {
        Task<Result<List<FarmerOrderDto>>> GetFarmerDashboardItemsAsync(int farmerId, ItemStatus itemStatus);
        Task<Result<int>> AcceptedOrderItemAsync(int id);
        Task<Result<int>> ReadyForPickUpOrderItemAsync(int id);
        Task<Result<int>> CanceledOrderItemAsync(int orderItemId);
    }
}
