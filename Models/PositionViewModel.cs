using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.Models
{
    public class PositionViewModel
    {
        public int EmployeeID { get; set; }
        public int PositionID { get; set; }
        public List<Position> PositionList { get; set; }
        public List<SelectListItem> Positions { get; set; }
    }
}
