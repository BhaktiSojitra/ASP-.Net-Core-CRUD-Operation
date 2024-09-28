namespace CRUD_Demo
{
    public class CommonVariable
    {
        private static IHttpContextAccessor _HttpContextAccessor;
        static CommonVariable()
        {
            _HttpContextAccessor = new HttpContextAccessor();
        }
        public static int? UserID()
        {
            if (_HttpContextAccessor.HttpContext.Session.GetString("UserID") == null)
            {
                return null;
            }
            return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("UserID"));
        }
        public static string UserName()
        {
            if (_HttpContextAccessor.HttpContext.Session.GetString("UserName") == null)
            {
                return null;
            }

            return _HttpContextAccessor.HttpContext.Session.GetString("UserName");
        }
        public static string Email()
        {
            if (_HttpContextAccessor.HttpContext.Session.GetString("Email") == null)
            {
                return null;
            }
            return _HttpContextAccessor.HttpContext.Session.GetString("Email");
        }
    }
}
