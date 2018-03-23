using ApiDemo.Models;
using Dapper;
using DapperExtensions.Extend.Contexts;
using DapperExtensions.Extend.Wraps;
using System.Data.Common;
using System.Web.Http;

namespace Apidemo.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        public IRespositoryBase<UserInfo> userRespo { get; set; }

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            //var list = userRespo.GetList(x => x.Age > 21, null);
            var list = userRespo.Context.Db.Query<UserInfo>("select * from [userinfo]");
            return Json(new { data = list });
        }

        [Route("add")]
        [HttpGet]
        public IHttpActionResult CreateUser()
        {
            var u = new UserInfo() { id = 1111111111, cnName = "fd13", age = 26 };
            //指定主键id
            var b = //userRespo.Insert(u);
            //不指定主键id
            userRespo.Insert(u, x => x.id);
            return Json(b);
        }

        [HttpGet]
        [Route("update")]
        public IHttpActionResult UpdateUser()
        {
            var f1 = new DbFiled<UserInfo>(x => x.age, 27);
            var b = userRespo.Update(x => x.id == 2, f1);
            return Json(b);
        }

        [HttpGet]
        [Route("one")]
        public IHttpActionResult GetUser()
        {
            var b = userRespo.Get(x => x.id, 1111111111);
            return Json(b);
        }

        [HttpGet]
        [Route("page")]
        public IHttpActionResult GetUserPaging()
        {
            var total = 0;
            Sorting<UserInfo>[] sorts = { new Sorting<UserInfo>(x => x.id, SortType.Desc) };
            var b = userRespo.GetPage(x => x.id > 0, sorts, 1, 5, false, ref total);
            return Json(b);
        }

        [HttpGet]
        [Route("test1")]
        public IHttpActionResult GetTest1()
        {
            using (SimpleDbConnection db = new SimpleDbConnection(userRespo.Context.Db))
            {

                var tran = db.BeginTransaction();
                var command = db.CreateCommand();

                command.CommandText = "select now();";
                var obj = command.ExecuteScalar();
                Exec(tran);
                tran.Commit();
            }

            using (SimpleDbConnection db = new SimpleDbConnection(userRespo.Context.Db))
            {

                var tran = db.BeginTransaction();
                var command = db.CreateCommand();

                command.CommandText = "select now();";
                command.ExecuteScalar();

                tran.Commit();
            }
            return Ok("xxx");

        }


        private int Exec(DbTransaction tran)
        {
            var command = tran.Connection.CreateCommand();
            command.CommandText = "update user SET Age=27 where id = 2";
            return command.ExecuteNonQuery();
        }
    }
}

