

namespace ApiDemo.Models
{
    public class user
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
    public class UserInfo
    {
        //id, cnName, enName, deptID, pwd, sex, age, qq, mobile, email, state, remark

        public int id { get; set; }
        public string cnName { get; set; }
        public string enName { get; set; }

        public int deptID { get; set; }

        public string pwd { get; set; }

        public int sex { get; set; }

        public int age { get; set; }

        public string qq { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public int state { get; set; }

        public string remark { get; set; }
    }
}