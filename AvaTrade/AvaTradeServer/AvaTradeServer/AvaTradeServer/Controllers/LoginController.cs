using AvaTradeServer.Models;
using LoginDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AvaTradeServer.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {

        public IEnumerable<Login> Get()
        {
            using (LoginDetailsEntities entities = new LoginDetailsEntities())
            {
                return entities.Login.ToList();
            }
        }


        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            using (LoginDetailsEntities db = new LoginDetailsEntities())
            {
                var userLogin = db.Set<Login>();
                userLogin.Add(new Login { login_time=DateTime.Now,user_name="test2" });

                db.SaveChanges();
            }
        }


        [HttpPost]
        [Route("api/check_login_user")]
 
        //  public LoginResponse CheckLoginUser([FromBody]string value)
        public LoginResponse CheckLoginUser(LoginRequest request)
        {
            try
            {
                /* LoginResponse res = new LoginResponse();
                 using (LoginDetailsEntities entities = new LoginDetailsEntities())
                 {
                   int countLofin = entities.Login.Where(x => x.login_time >= DateTime.Now.AddMinutes(-5)).Count();
                   res.isBlocked = true;
                     //return entities.Login.ToList();
                 }*/

                return UserLogin.Instance.CheckLoginUser(request.userName, request.password);
            }
            catch (Exception ex)
            {
                LoginResponse res = new LoginResponse() { isLoginSuccess = false };
                return res;

            }
        }

    }



}
