using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Core;
using MOI.Patrol.DataAccessLayer;
namespace MOI.Patrol.Controllers
{
    [Route("api/[controller]")]

    public class GenericController : Controller
    {
        private Handler_Person _person = new Handler_Person();
        private Handler_User _user = new Handler_User();
        private Handler_PatrolCars _patrol = new Handler_PatrolCars();
        private Handler_HandHelds _handheld = new Handler_HandHelds();
        private Handler_AhwalMapping _ahwalmapping = new Handler_AhwalMapping();
        private patrolsContext _context = new patrolsContext();

        private String constr = "server=localhost;Port=5432;User Id=postgres;password=12345;Database=Patrols";
        private DataAccess DAL = new DataAccess();

        [HttpPost("ahwalList")]
        public IActionResult PostAhwalList([FromBody] int userid)
        {
            // var results = _context.Database.SqlQuery<string>("SELECT ahwalid,name FROM Ahwal WHERE table_name = @p0", tableName).ToArray();
           // var results = _context.Database.SqlQuery<string>("SELECT ahwalid,name FROM Ahwal WHERE table_name = @p0", tableName).ToArray();
            var results = _context.Ahwal.ToList();

            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpGet("shiftsList")]
        public IActionResult GetShiftsList()
        {
            // String Qry = "SELECT ShiftID, Name, StartingHour, NumberOfHours FROM Shifts";
            var results = _context.Shifts.ToList();
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpPost("checkinoutstates")]
        public IActionResult GetCheckInOutStates()
        {
            // String Qry = "SELECT ShiftID, Name, StartingHour, NumberOfHours FROM Shifts";
            var results = _context.Checkinoutstates.ToList();
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return Ok(null);
            }
        }

    }
}