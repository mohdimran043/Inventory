using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Elasticsearch.Net;
using Nest;
using System.Reflection;
//using CrystalDecisions.CrystalReports.Engine;


namespace MOI.AssetManagement.Controllers {
    public class FileUploadVwModal
    {
       public IFormFile File { get; set; }
        public long Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }


    public class Driver
    {

        public string name { get; set; }
        public string idnumber { get; set; }
        public string telnumber { get; set; }
        public string empNumber { get; set; }


    }

    public class Device
    {

        public string name { get; set; }
        public string idnumber { get; set; }
        public string telnumber { get; set; }
        public string empNumber { get; set; }
        public string deviceid { get; set; }
        public string LngLat { get; set; }
    }

    public class vehicledrivercls
    {

        public string vehicleplateno { get; set; }
        public string deviceno { get; set; }
        public string driverId { get; set; }
        public string devicemodel { get; set; }
        public string IMSI { get; set; }
        public string IMEI { get; set; }
        public string userid { get; set; }
        public string drivername { get; set; }
    }

    public class QryString
{
    public string Qry { get; set; }
}

   public class vehiclecls
    {
        public string plateno { get; set; }
        public string gsmno { get; set; }
        public string vehclass { get; set; }
        public string brand { get; set; }
        public string type { get; set; }
        public string model { get; set; }
        public string notes { get; set; }
        public string userid { get; set; }
    }

    /*   public class DriverInfo
       {
           public Int64 Id { get; set; },
            public string Name { get; set; }
       }*/

    [Route("api/[controller]")]
    public class MapController : Controller
    {

        public static string LngLat = "Rameez";
        //  public String constr = "Server=BCI666016PC57;Database=MOI_Assets;Integrated Security=true;User Id=Asset User;Password=12345;Trusted_Connection=True;MultipleActiveResultSets=true";
        public String constr2 = "Server=BCI666016PC57;Database=MOI_Assets;User Id =AssetUser;Password=12345;";

       // public IConfiguration _connectionstring { get; }
       // public String constr2;
      /*  public MapController(IConfiguration configuration)
        {
            _connectionstring = configuration;
            constr2 = _connectionstring["ConnectionStrings:DefaultConnection"];
        }*/

        [HttpPost("DriverQry")]
        public DataTable PostDriverQuery([FromBody] Driver dcls)
        {
            string w_clause = "";
            if (dcls.name !="" && dcls.name !="undefined" && dcls.name !=null)
            {
                w_clause = w_clause + " and name like '%" + dcls.name + "%'";
            }

            if (dcls.idnumber != "" && dcls.idnumber != "undefined" && dcls.idnumber != null)
            {
                w_clause = w_clause + " and idnumber like '%" + dcls.idnumber + "%'";
            }

            if (dcls.empNumber != "" && dcls.empNumber != "undefined" && dcls.empNumber != null)
            {
                w_clause = w_clause + " and empNumber like '%" + dcls.empNumber + "%'";
            }

            if (dcls.telnumber != "" && dcls.telnumber != "undefined" && dcls.telnumber != null)
            {
                w_clause = w_clause + " and telnumber like '%" + dcls.telnumber + "%'";
            }


            if (w_clause !="")
            {
                w_clause = " where " + w_clause.Substring(4);

            }
            SqlConnection cont = new SqlConnection();
            cont.ConnectionString = constr2;
            cont.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from driverinfo " + w_clause, cont);
            da.Fill(dt);
            cont.Close();
            cont.Dispose();
            //  List <DriverInfo> = new List<DriverInfo>;

            return dt;
        }

        [HttpPost("SaveDriverDtl")]
        public string PostSaveData([FromBody] Driver dcls)
        {
            SqlConnection cont = new SqlConnection();
            cont.ConnectionString = constr2;
            cont.Open();
            DataTable dt = new DataTable();
            //SqlDataAdapter da = new SqlDataAdapter("select * from driverinfo " + HttpUtility.HtmlDecode(w_clause), cont);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cont;
            cmd.CommandText = " update driverinfo set idnumber='" + dcls.idnumber + "' ,telnumber='" + dcls.telnumber + "' where name='" + dcls.name + "'";
            cmd.ExecuteNonQuery();
            //da.Fill(dt);
            cont.Close();
            cont.Dispose();
            return "Saved Successfully";
        }

        [HttpPost]
        public string PostUpload(List<IFormFile> files)
        {
            IFormFile item = files[0];
            // foreach (IFormFile item in files)
            //{
            string filename = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
            filename = this.EnsureFileame(filename);
            using (FileStream filestream = System.IO.File.Create(this.GetPath(filename)))
            {
                item.CopyTo(filestream);
            }
            // }
            return "assets/images/" + filename;
        }

        private string GetPath(string filename)
        {
            string path = "D:\\Projects\\Thaabet\\src\\QuickApp\\ClientApp\\src\\assets\\images\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + filename;
        }

        private string EnsureFileame(string filename)
        {
            if (filename.Contains("\\"))
            {
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            }
            return filename;
        }

        /*  public async Task<IActionResult> Post(List<IFormFile> files)
          {
              long size = files.Sum(f => f.Length);

              // full path to file in temp location
              var filePath = Path.GetTempFileName();

              foreach (var formFile in files)
              {
                  if (formFile.Length > 0)
                  {
                      using (var stream = new FileStream(filePath, FileMode.Create))
                      {
                          await formFile.CopyToAsync(stream);
                      }
                  }
              }


              return Ok(new { count = files.Count, size, filePath });
          }*/


        [HttpGet("DriverElastic")]
        public DataTable GetDriverElasticQuery(String w_clause)
        {
            ElasticClient es = new ElasticClient();
            es = EsClient();
            QueryContainer ContQry = new QueryContainer();
            MatchAllQuery Qry1 = new MatchAllQuery();
            Qry1.Name = "Qry1";

            ContQry = Qry1;
            SearchRequest esserach = new SearchRequest();
            esserach.Query = ContQry;
//
          //  SearchDescriptor<Device> esdesct = new SearchDescriptor<Device>();



            // var esresponse = new SearchResponse<Driver>();
            var esresponse = es.Search<Device>(esserach);
            DataTable dt = new DataTable();
            if (esresponse.Documents.Count > 0)
            {
                dt = ListToDataTable<Device>(esresponse.Documents.ToList());
            }


            return dt;
        }

        [HttpGet("AllVehiclesElastic")]
        public string GetAllVehiclesElastic(String vehicles)
        {
            ElasticClient es = new ElasticClient();
            es = EsClient();
            QueryContainer ContQry = new QueryContainer();
            MatchQuery Qry1 = new MatchQuery();
            Qry1.Field = "deviceid";
            Qry1.Operator = Operator.And;
            Qry1.Query = vehicles;
            ContQry = Qry1;
            SearchRequest esserach = new SearchRequest();
            esserach.Query = ContQry;

            //SearchDescriptor<Driver> esdesct = new SearchDescriptor<Driver>();



            // var esresponse = new SearchResponse<Driver>();
            var esresponse = es.Search<Device>(esserach);
             DataTable dt = new DataTable();
            string lnglat = "";
            if (esresponse.Documents.Count > 0)
            {
                 dt = ListToDataTable<Device>(esresponse.Documents.ToList());
                lnglat = dt.Rows[dt.Rows.Count-1]["LngLat"].ToString();
            }


            return lnglat;
        }

        [HttpGet("DeviceRoute")]
        public DataTable GetDeviceRoute(String FromDt, String ToDt)
        {
            

            ElasticClient es = new ElasticClient();
            es = EsClient();
            QueryContainer ContQry = new QueryContainer();
            MatchQuery Qry1 = new MatchQuery();
            Qry1.Field = "deviceid";
            Qry1.Operator = Operator.And;
            Qry1.Query = "26470";
            ContQry = Qry1;
            SearchRequest esserach = new SearchRequest();
            esserach.Query = ContQry;

            var esresponse = es.Search<Device>(esserach);
            DataTable dt = new DataTable();
            //string lnglat = "";
            if (esresponse.Documents.Count > 0)
            {
                dt = ListToDataTable<Device>(esresponse.Documents.ToList());
                //lnglat = dt.Rows[dt.Rows.Count - 1]["LngLat"].ToString();
            }

            return dt;
        }


        [HttpGet("PrintDtls")]
        public string PrintDtls(String w_clause)
        {
            String rptname = "";
           // ReportDocument rd = new ReportDocument();
          //  rd.Load("G:\\Thabeet\\BackUpThabeetIMO\\src\\QuickApp\\CrystalReports\\Report1.Rpt");

           // rd.SetDataSource(GetDriverQuery(""));

            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();
            return "/assets/Attachments/" + rptname;
        }
        #region Connection string to connect with Elasticsearch  

        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public ElasticClient EsClient()
        {
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
            };

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
            connectionSettings.DefaultIndex("driverinfo");
            var elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }

        #endregion Connection string to connect with Elasticsearch  

        [HttpPost("VehicleDriverList")]
        public DataTable PostVehicleDriverQuery([FromBody] vehicledrivercls dcls)
        {
            string w_clause = "";
            if (dcls.drivername != "" && dcls.drivername != "undefined" && dcls.drivername != null)
            {
                w_clause = w_clause + " and DriverId in (select id from driverinfo where name like '%" + dcls.drivername + "%')";
            }

            if (dcls.devicemodel != "" && dcls.devicemodel != "undefined" &&  dcls.devicemodel != null)
            {
                w_clause = w_clause + " and DeviceModel like '%" + dcls.devicemodel + "%'";
            }

            if (dcls.deviceno != "" && dcls.deviceno != "undefined" &&  dcls.deviceno != null)
            {
                w_clause = w_clause + " and DeviceNo like '%" + dcls.deviceno + "%'";
            }

            if (dcls.IMSI != "" && dcls.IMSI != "undefined" &&  dcls.IMSI != null)
            {
                w_clause = w_clause + " and IMSI like '%" + dcls.IMSI + "%'";
            }

            if (dcls.IMEI != "" && dcls.IMEI != "undefined" &&  dcls.IMEI != null)
            {
                w_clause = w_clause + " and IMEI like '%" + dcls.IMEI + "%'";
            }

            if (dcls.vehicleplateno != "" && dcls.vehicleplateno != "undefined" && dcls.vehicleplateno != null)
            {
                w_clause = w_clause + " and vehicleplateno like '%" + dcls.vehicleplateno + "%'";
            }


            if (w_clause != "")
            {
                w_clause = " where " + w_clause.Substring(4);
            }

            SqlConnection cont = new SqlConnection();
            cont.ConnectionString = constr2;
            cont.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from VehicleDriver " + w_clause, cont);
            da.Fill(dt);
            cont.Close();
            cont.Dispose();
            //  List <DriverInfo> = new List<DriverInfo>;

            return dt;
        }

        [HttpPost("DelVehicleDriver")]
        public string PostDelVehicleDriver([FromBody] vehicledrivercls dcls)
        {

            SqlConnection cont = new SqlConnection();
            cont.ConnectionString = constr2;
            cont.Open();
            DataTable dt = new DataTable();
            //SqlDataAdapter da = new SqlDataAdapter("select * from driverinfo " + HttpUtility.HtmlDecode(w_clause), cont);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cont;
            cmd.CommandText = " delete vehicledriver  where deviceno='" + dcls.deviceno + "' and vehicleplateno='" + dcls.vehicleplateno + "'";
            cmd.ExecuteNonQuery();
            //da.Fill(dt);
            cont.Close();
            cont.Dispose();
            return "Deleted Successfully";
        }

        [HttpPost("InsVehicleDriver")]
        public string PostInsVehicleDriver([FromBody] vehicledrivercls dcls)
        {
            try
            {
                SqlConnection cont = new SqlConnection();
                cont.ConnectionString = constr2;
                cont.Open();
                // DataTable dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("select * from driverinfo " + HttpUtility.HtmlDecode(w_clause), cont);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cont;
                cmd.CommandText = " insert into vehicledriver(deviceno,vehicleplateno,driverid,devicemodel,imsi,imei,userid) values('" + dcls.deviceno + "','" + dcls.vehicleplateno + "','" + dcls.driverId + "','" + dcls.devicemodel + "','" + dcls.IMSI + "','" + dcls.IMEI + "','" + dcls.userid + "')";
                int Records = cmd.ExecuteNonQuery();
                //da.Fill(dt);
                cont.Close();
                cont.Dispose();
                return Records + " Saved Successfully";
            }
            catch(Exception e)
            {
                return "error" + e.Message;
            }

           
        }

        [HttpPost("VehicleList")]
        public DataTable PostVehicleQuery([FromBody] vehiclecls dcls)
        {
            string w_clause = "";
          
            if (dcls.plateno != "" && dcls.plateno != "undefined" && dcls.plateno != null)
            {
                w_clause = w_clause + " and PlateNo like '%" + dcls.plateno + "%'";
            }

            if (dcls.gsmno != "" && dcls.gsmno != "undefined" && dcls.gsmno != null)
            {
                w_clause = w_clause + " and gsmno like '%" + dcls.gsmno + "%'";
            }

            if (dcls.vehclass != "" && dcls.vehclass != "undefined" && dcls.vehclass != null)
            {
                w_clause = w_clause + " and Class like '%" + dcls.vehclass + "%'";
            }

            if (dcls.brand != "" && dcls.brand != "undefined" && dcls.brand != null)
            {
                w_clause = w_clause + " and brand like '%" + dcls.brand + "%'";
            }

            if (dcls.model != "" && dcls.model != "undefined" && dcls.model != null)
            {
                w_clause = w_clause + " and model like '%" + dcls.model + "%'";
            }

            if (dcls.notes != "" && dcls.notes != "undefined" && dcls.notes != null)
            {
                w_clause = w_clause + " and notes like '%" + dcls.notes + "%'";
            }

            if (dcls.type != "" && dcls.type != "undefined" && dcls.type != null)
            {
                w_clause = w_clause + " and type like '%" + dcls.type + "%'";
            }

           
            if (w_clause != "")
            {
                w_clause = " where " + w_clause.Substring(4);
            }

            SqlConnection cont = new SqlConnection();
            cont.ConnectionString = constr2;
            cont.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from Vehicle " + w_clause, cont);
            da.Fill(dt);
            cont.Close();
            cont.Dispose();
            //  List <DriverInfo> = new List<DriverInfo>;

            return dt;
        }

        [HttpPost("DelVehicle")]
        public string PostDelVehicle([FromBody] vehiclecls dcls)
        {

            SqlConnection cont = new SqlConnection();
            cont.ConnectionString = constr2;
            cont.Open();
            DataTable dt = new DataTable();
            //SqlDataAdapter da = new SqlDataAdapter("select * from driverinfo " + HttpUtility.HtmlDecode(w_clause), cont);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cont;
            cmd.CommandText = " delete Vehicle  where plateno='" + dcls.plateno + "'";
            cmd.ExecuteNonQuery();
            //da.Fill(dt);
            cont.Close();
            cont.Dispose();
            return "Deleted Successfully";
        }

        [HttpPost("InsVehicle")]
        public string PostInsVehicle([FromBody] vehiclecls dcls)
        {
            try
            {
                SqlConnection cont = new SqlConnection();
                cont.ConnectionString = constr2;
                cont.Open();
                // DataTable dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter("select * from driverinfo " + HttpUtility.HtmlDecode(w_clause), cont);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cont;
                cmd.CommandText = " insert into vehicle(plateno,gsmno,class,brand,type,model,notes,savedby) values('" + dcls.plateno + "','" + dcls.gsmno + "','" + dcls.vehclass + "','" + dcls.brand + "','" + dcls.type + "','" + dcls.model + "','" + dcls.notes + "','" + dcls.userid + "')";
                int Records = cmd.ExecuteNonQuery();
                //da.Fill(dt);
                cont.Close();
                cont.Dispose();
                return Records + " Saved Successfully";
            }
            catch (Exception e)
            {
                return "error" + e.Message;
            }


        }

    }


}