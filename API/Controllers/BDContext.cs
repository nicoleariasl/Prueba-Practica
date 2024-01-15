using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BDContext : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public BDContext(IConfiguration configuration)
        {
            _configuration = configuration;

        }

       /* [HttpGet]
        [Route("getAllUsers")]
        public JsonResult GetUser()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("BDContext").ToString());
            SqlDataAdapter da = new SqlDataAdapter("Select * from Usuarios", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
        }*/
    }
}
